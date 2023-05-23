using Microsoft.EntityFrameworkCore;
using NgSoftware.Specular.Administrations.Entities;
using NgSoftware.Specular.Administrations.Repositories.Contracts;
using NgSoftware.Specular.Common.Repositories;
using NgSoftware.Specular.Context.Contracts;

namespace NgSoftware.Specular.Administrations.Repositories;

/// <inheritdoc cref="IUserOrganizationReadRepository"/>
public class UserOrganizationReadRepository : IUserOrganizationReadRepository, IAdministrationRepositoryAnchor
{
    private readonly IReader reader;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="UserOrganizationReadRepository"/>
    /// </summary>
    public UserOrganizationReadRepository(IReader reader)
    {
        this.reader = reader;
    }

    Task<UserOrganization?> IUserOrganizationReadRepository.GetByUserAndOrganizationIdAsync(Guid userId,
        Guid organizationId,
        CancellationToken cancellationToken)
        => reader.Read<UserOrganization>()
            .Where(x => x.UserId == userId &&
                        x.OrganizationId == organizationId)
            .NotDeletedAt()
            .FirstOrDefaultAsync(cancellationToken);

    Task<IReadOnlyCollection<UserOrganization>> IUserOrganizationReadRepository.GetByOrganizationIdAsync(Guid organizationId,
        CancellationToken cancellationToken)
        => reader.Read<UserOrganization>()
            .Where(x => x.OrganizationId == organizationId)
            .NotDeletedAt()
            .ToReadOnlyCollectionAsync(cancellationToken);
}
