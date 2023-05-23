using NgSoftware.Specular.Administrations.Entities;
using NgSoftware.Specular.Administrations.Repositories.Contracts;
using NgSoftware.Specular.Common.Repositories;
using NgSoftware.Specular.Context.Contracts;

namespace NgSoftware.Specular.Administrations.Repositories;

/// <inheritdoc cref="IRefreshTokenReadRepository"/>
public class TeamReadRepository : ITeamReadRepository, IAdministrationRepositoryAnchor
{
    private readonly IReader reader;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="TeamReadRepository"/>
    /// </summary>
    public TeamReadRepository(IReader reader)
    {
        this.reader = reader;
    }

    Task<IReadOnlyCollection<Team>> ITeamReadRepository.GetByOrganizationIdAsync(Guid organizationId, CancellationToken cancellationToken)
        => reader.Read<Team>()
            .Where(x => x.OrganizationId == organizationId)
            .NotDeletedAt()
            .ToReadOnlyCollectionAsync(cancellationToken);
}
