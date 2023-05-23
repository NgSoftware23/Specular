using NgSoftware.Specular.Administrations.Entities;
using NgSoftware.Specular.Administrations.Repositories.Contracts;
using NgSoftware.Specular.Administrations.Services.Contracts.Exceptions;
using NgSoftware.Specular.Administrations.Services.Contracts.Interfaces;
using NgSoftware.Specular.Administrations.Services.Contracts.Models.Token;
using NgSoftware.Specular.Common.Core.Contracts;
using NgSoftware.Specular.Context.Contracts;

namespace NgSoftware.Specular.Administrations.Services.User;

/// <inheritdoc cref="IRefreshTokenManager"/>
public class RefreshTokenManager : IRefreshTokenManager, IAdministrationsServiceAnchor
{
    private readonly IRefreshTokenReadRepository refreshTokenReadRepository;
    private readonly IRefreshTokenWriteRepository refreshTokenWriteRepository;
    private readonly IUnitOfWork unitOfWork;
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly IUserReadRepository userReadRepository;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="RefreshTokenManager"/>
    /// </summary>
    public RefreshTokenManager(IRefreshTokenReadRepository refreshTokenReadRepository,
        IRefreshTokenWriteRepository refreshTokenWriteRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        IUserReadRepository userReadRepository)
    {
        this.refreshTokenReadRepository = refreshTokenReadRepository;
        this.refreshTokenWriteRepository = refreshTokenWriteRepository;
        this.unitOfWork = unitOfWork;
        this.dateTimeProvider = dateTimeProvider;
        this.userReadRepository = userReadRepository;
    }

    async Task<Guid> IRefreshTokenManager.CreateRefreshTokenAsync(CreateRefreshTokenModel model, CancellationToken cancellationToken)
    {
        var oldToken = await refreshTokenReadRepository.GetActualByUserIdAsync(model.UserId, cancellationToken);
        if (oldToken != null)
        {
            refreshTokenWriteRepository.Delete(oldToken);
        }

        var moment = dateTimeProvider.UtcNow;
        var token = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = model.UserId,
            AccessPayload = model.AccessPayload,
            CreatedAt = moment,
            SecurityStamp = model.SecurityStamp,
            Expires = moment.AddDays(model.ExpiredDays),
        };
        refreshTokenWriteRepository.Add(token);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return token.Id;
    }

    async Task<UpdateRefreshTokenModel> IRefreshTokenManager.UpdateRefreshTokenAsync(Guid tokenId, CancellationToken cancellationToken)
    {
        var currentToken = await refreshTokenReadRepository.GetActualByIdAsync(tokenId, cancellationToken);
        if (currentToken == null)
        {
            throw new AdministrationEntityNotFoundException<RefreshToken>(tokenId);
        }

        if (currentToken.Expires <= dateTimeProvider.UtcNow)
        {
            refreshTokenWriteRepository.Delete(currentToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            throw new AdministrationInvalidOperationException("Срок действия токена окончен");
        }

        var user = await userReadRepository.GetByIdAsync(currentToken.UserId, cancellationToken);
        if (user == null ||
            user.SecurityStamp != currentToken.SecurityStamp ||
            user.PasswordIsTemporary)
        {
            refreshTokenWriteRepository.Delete(currentToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            throw new AdministrationInvalidOperationException("Невалидный токен. Отметка безопасности пользователя изменена");
        }

        refreshTokenWriteRepository.Delete(currentToken);
        var moment = dateTimeProvider.UtcNow;
        var token = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = currentToken.UserId,
            AccessPayload = currentToken.AccessPayload,
            CreatedAt = moment,
            SecurityStamp = currentToken.SecurityStamp,
            Expires = moment.AddDays((currentToken.Expires - currentToken.CreatedAt).TotalDays),
        };
        refreshTokenWriteRepository.Add(token);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new UpdateRefreshTokenModel
        {
            ClaimPayload = token.AccessPayload,
            TokenId = token.Id,
        };
    }

    async Task IRefreshTokenManager.DeleteRefreshTokenByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var currentToken = await refreshTokenReadRepository.GetActualByUserIdAsync(userId, cancellationToken);
        if (currentToken != null)
        {
            refreshTokenWriteRepository.Delete(currentToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
