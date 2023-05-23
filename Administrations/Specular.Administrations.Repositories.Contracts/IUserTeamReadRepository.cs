using NgSoftware.Specular.Administrations.Entities;

namespace NgSoftware.Specular.Administrations.Repositories.Contracts;

/// <summary>
/// Репозиторий на чтение <see cref="UserTeam"/>
/// </summary>
public interface IUserTeamReadRepository
{
    /// <summary>
    /// Получает список <see cref="UserTeam"/> по идентификатору организации
    /// </summary>
    Task<IReadOnlyCollection<UserTeam>> GetByTeamIdsAsync(IReadOnlyCollection<Guid> teamIds, CancellationToken cancellationToken);
}
