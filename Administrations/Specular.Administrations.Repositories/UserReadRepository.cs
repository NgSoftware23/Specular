using Microsoft.EntityFrameworkCore;
using NgSoftware.Specular.Administrations.Entities;
using NgSoftware.Specular.Administrations.Repositories.Contracts;
using NgSoftware.Specular.Common.Repositories;
using NgSoftware.Specular.Context.Contracts;

namespace NgSoftware.Specular.Administrations.Repositories;

/// <inheritdoc cref="IUserReadRepository"/>
public class UserReadRepository : IUserReadRepository, IAdministrationRepositoryAnchor
{
    private readonly IReader reader;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="UserReadRepository"/>
    /// </summary>
    public UserReadRepository(IReader reader)
    {
        this.reader = reader;
    }

    Task<User?> IUserReadRepository.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => reader.Read<User>()
            .ById(id)
            .SingleOrDefaultAsync(cancellationToken);

    Task<User?> IUserReadRepository.GetActiveByIdAsync(Guid id, CancellationToken cancellationToken)
        => reader.Read<User>()
            .NotDeletedAt()
            .ById(id)
            .Where(x => !x.Blocked)
            .SingleOrDefaultAsync(cancellationToken);

    Task<User?> IUserReadRepository.GetActiveByLoginAsync(string login, CancellationToken cancellationToken)
        => reader.Read<User>()
            .NotDeletedAt()
            .Where(x => x.LoginLowerCase == login.ToLower())
            .SingleOrDefaultAsync(cancellationToken);

    Task<User?> IUserReadRepository.GetActiveByMailAsync(string email, CancellationToken cancellationToken)
        => reader.Read<User>()
            .NotDeletedAt()
            .Where(x => x.EmailLowerCase == email.ToLower())
            .SingleOrDefaultAsync(cancellationToken);


    Task<bool> IUserReadRepository.IsActiveLoginExistsAsync(string login, CancellationToken cancellationToken)
        => reader.Read<User>()
            .NotDeletedAt()
            .AnyAsync(x => x.LoginLowerCase == login.ToLower(), cancellationToken);

    Task<bool> IUserReadRepository.IsActiveEmailExistsAsync(string email, CancellationToken cancellationToken)
        => reader.Read<User>()
            .NotDeletedAt()
            .AnyAsync(x => x.EmailLowerCase == email.ToLower() &&
                           x.EmailConfirmed,
                cancellationToken);

    Task<IReadOnlyCollection<User>> IUserReadRepository.GetByOrganizationIdAsync(Guid organizationId, CancellationToken cancellationToken)
        => reader.Read<User>()
            .Where(x => x.Organizations.Any(y => y.OrganizationId == organizationId))
            .ToReadOnlyCollectionAsync(cancellationToken);
}
