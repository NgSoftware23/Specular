using NgSoftware.Specular.Administrations.Entities;

namespace NgSoftware.Specular.Administrations.Repositories.Contracts;

/// <summary>
/// Репозиторий на чтение <see cref="Team"/>
/// </summary>
public interface ITeamReadRepository
{
    /// <summary>
    /// Получает список <see cref="Team"/> по идентификатору организации
    /// </summary>
    Task<IReadOnlyCollection<Team>> GetByOrganizationIdAsync(Guid organizationId, CancellationToken cancellationToken);
}
