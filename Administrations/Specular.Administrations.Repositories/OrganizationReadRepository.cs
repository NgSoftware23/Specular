using Microsoft.EntityFrameworkCore;
using NgSoftware.Specular.Administrations.Entities;
using NgSoftware.Specular.Administrations.Repositories.Contracts;
using NgSoftware.Specular.Common.Repositories;
using NgSoftware.Specular.Context.Contracts;

namespace NgSoftware.Specular.Administrations.Repositories;

/// <inheritdoc cref="IOrganizationReadRepository"/>
public class OrganizationReadRepository : IOrganizationReadRepository, IAdministrationRepositoryAnchor
{
    private readonly IReader reader;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="OrganizationReadRepository"/>
    /// </summary>
    public OrganizationReadRepository(IReader reader)
    {
        this.reader = reader;
    }

    Task<Organization?> IOrganizationReadRepository.GetActiveByIdAsync(Guid id, CancellationToken cancellationToken)
        => reader.Read<Organization>()
            .ById(id)
            .NotDeletedAt()
            .FirstOrDefaultAsync(cancellationToken);

    Task<IReadOnlyCollection<Organization>> IOrganizationReadRepository.GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        => reader.Read<Organization>()
            .Where(x => x.Users.Any(y => y.UserId == userId &&
                                         y.DeletedAt == null))
            .NotDeletedAt()
            .OrderBy(x => x.NameLowerCase)
            .ToReadOnlyCollectionAsync(cancellationToken);

    Task<bool> IOrganizationReadRepository.IsActiveNameExistsAsync(string organizationName, CancellationToken cancellationToken)
        => reader.Read<Organization>()
            .Where(x => x.NameLowerCase == organizationName.ToLower())
            .NotDeletedAt()
            .AnyAsync(cancellationToken);
}
