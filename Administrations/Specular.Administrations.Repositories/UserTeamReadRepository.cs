using System.Linq.Expressions;
using NgSoftware.Specular.Administrations.Entities;
using NgSoftware.Specular.Administrations.Repositories.Contracts;
using NgSoftware.Specular.Common.Repositories;
using NgSoftware.Specular.Context.Contracts;

namespace NgSoftware.Specular.Administrations.Repositories;

/// <inheritdoc cref="IUserTeamReadRepository"/>
public class UserTeamReadRepository : IUserTeamReadRepository, IAdministrationRepositoryAnchor
{
    private readonly IReader reader;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="UserTeamReadRepository"/>
    /// </summary>
    public UserTeamReadRepository(IReader reader)
    {
        this.reader = reader;
    }

    Task<IReadOnlyCollection<UserTeam>> IUserTeamReadRepository.GetByTeamIdsAsync(IReadOnlyCollection<Guid> teamIds,
        CancellationToken cancellationToken)
        => reader.Read<UserTeam>()
            .Where(GetByTeamsIdsExpression(teamIds))
            .NotDeletedAt()
            .ToReadOnlyCollectionAsync(cancellationToken);

    private static Expression<Func<UserTeam, bool>> GetByTeamsIdsExpression(IReadOnlyCollection<Guid> ids)
        => ids.Count switch
        {
            0 => x => false,
            1 => x => x.TeamId == ids.First(),
            _ => x => ids.Contains(x.TeamId)
        };
}
