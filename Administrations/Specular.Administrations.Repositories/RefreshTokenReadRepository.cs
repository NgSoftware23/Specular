using Microsoft.EntityFrameworkCore;
using NgSoftware.Specular.Administrations.Entities;
using NgSoftware.Specular.Administrations.Repositories.Contracts;
using NgSoftware.Specular.Common.Repositories;
using NgSoftware.Specular.Context.Contracts;

namespace NgSoftware.Specular.Administrations.Repositories;

/// <inheritdoc cref="IRefreshTokenReadRepository"/>
public class RefreshTokenReadRepository : IRefreshTokenReadRepository, IAdministrationRepositoryAnchor
{
    private readonly IReader reader;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="RefreshTokenReadRepository"/>
    /// </summary>
    public RefreshTokenReadRepository(IReader reader)
    {
        this.reader = reader;
    }

    Task<RefreshToken?> IRefreshTokenReadRepository.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => reader.Read<RefreshToken>()
            .ById(id)
            .FirstOrDefaultAsync(cancellationToken);

    Task<RefreshToken?> IRefreshTokenReadRepository.GetActualByIdAsync(Guid id, CancellationToken cancellationToken)
        => reader.Read<RefreshToken>()
            .ById(id)
            .NotDeletedAt()
            .FirstOrDefaultAsync(cancellationToken);

    Task<RefreshToken?> IRefreshTokenReadRepository.GetActualByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        => reader.Read<RefreshToken>()
            .NotDeletedAt()
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync(cancellationToken);
}
