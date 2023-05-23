using AutoMapper;
using NgSoftware.Specular.Administrations.Repositories.Contracts;
using NgSoftware.Specular.Administrations.Services.Contracts.Exceptions;
using NgSoftware.Specular.Administrations.Services.Contracts.Interfaces;
using NgSoftware.Specular.Administrations.Services.Contracts.Models.User;
using NgSoftware.Specular.Administrations.Services.Helper;
using NgSoftware.Specular.Administrations.Services.Resources;
using NgSoftware.Specular.Common.Core.Contracts;
using NgSoftware.Specular.Context.Contracts;

namespace NgSoftware.Specular.Administrations.Services.User;

/// <inheritdoc cref="IUserManager"/>
public class UserManager : IUserManager, IAdministrationsServiceAnchor
{
    private readonly IUserReadRepository userReadRepository;
    private readonly IUserWriteRepository userWriteRepository;
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="UserManager"/>
    /// </summary>
    public UserManager(IUserReadRepository userReadRepository,
        IUserWriteRepository userWriteRepository,
        IDateTimeProvider dateTimeProvider,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        this.userReadRepository = userReadRepository;
        this.userWriteRepository = userWriteRepository;
        this.dateTimeProvider = dateTimeProvider;
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    async Task IUserManager.CreateUserAsync(CreateUserModel model, CancellationToken cancellationToken)
    {
        var exist = await userReadRepository.IsActiveLoginExistsAsync(model.Login.ToLower(), cancellationToken);
        if (exist)
        {
            throw new AdministrationInvalidOperationException($"Пользователь с логином {model.Login} уже существует");
        }

        var saltValue = SecurityHelper.GenerateSalt32();
        var passwordHash = SecurityHelper.HashPassword32(model.Password, saltValue);
        var user = new Entities.User
        {
            Id = Guid.NewGuid(),
            Name = model.Name,
            Email = model.Email.Trim(),
            EmailLowerCase = model.Email.Trim().ToLower(),
            EmailConfirmed = true,
            Login = model.Login.Trim(),
            LoginLowerCase = model.Login.Trim().ToLower(),
            SecurityStamp = Guid.NewGuid().ToString(),
            PasswordHash = passwordHash,
            PasswordSalt = saltValue,
            PasswordIsTemporary = false,
        };
        userWriteRepository.Add(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    async Task<UserLoggedModel> IUserManager.GetActiveByLoginAndPasswordAsync(LoginModel model, CancellationToken cancellationToken)
    {
        var message = "Пользователь с указанным логином и паролем не найден";
        var user = await userReadRepository.GetActiveByLoginAsync(model.Login.ToLower(), cancellationToken);
        if (user == null)
        {
            throw new AdministrationNotFoundException(message);
        }

        if (user.Blocked && (!user.BlockedAt.HasValue || user.BlockedAt > dateTimeProvider.UtcNow))
        {
            throw new AdministrationNotFoundException(message);
        }

        var passwordHash = SecurityHelper.HashPassword32(model.Password, user.PasswordSalt);
        if (passwordHash != user.PasswordHash)
        {
            user.LoginAttemptFailedCount++;
            if (user.LoginAttemptFailedCount >= AdministrationConstants.MaximumFailedCount)
            {
                user.Blocked = true;
                user.BlockedAt = dateTimeProvider.UtcNow.AddMinutes(AdministrationConstants.LoginAttemptMinutesDelay);
                if (string.IsNullOrWhiteSpace(user.Note))
                {
                    user.Note = AdministrationConstants.FailedCountMessage;
                }
            }
            userWriteRepository.Update(user);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            throw new AdministrationNotFoundException(message);
        }

        if (user.LoginAttemptFailedCount > 0 || user.Blocked)
        {
            user.LoginAttemptFailedCount = 0;
            user.Blocked = false;
            user.BlockedAt = null;
            if (!string.IsNullOrEmpty(user.Note) &&
                user.Note.Equals(AdministrationConstants.FailedCountMessage, StringComparison.InvariantCulture))
            {
                user.Note = string.Empty;
            }

            userWriteRepository.Update(user);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return mapper.Map<UserLoggedModel>(user);
    }

    Task IUserManager.DeleteUserAsync(Guid id, CancellationToken cancellationToken) => throw new NotImplementedException();

    Task IUserManager.BlockUserAsync(BlockUserModel model, CancellationToken cancellationToken) => throw new NotImplementedException();

    Task IUserManager.UnBlockUserAsync(Guid id, CancellationToken cancellationToken) => throw new NotImplementedException();

    Task<IEnumerable<UserInfoModel>> IUserManager.GetUsersByOrganizationIdAsync(Guid organizationId, CancellationToken cancellationToken) => throw new NotImplementedException();

    Task IUserManager.SetNoteAsync(UserNoteModel model, CancellationToken cancellationToken) => throw new NotImplementedException();

    Task<UserLoggedModel> IUserManager.ModifyUserAsync(UserModifyModel model, CancellationToken cancellationToken) => throw new NotImplementedException();
}
