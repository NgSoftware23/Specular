using NgSoftware.Specular.Administrations.Entities;

namespace NgSoftware.Specular.Administrations.Repositories.Contracts;

/// <summary>
/// Репозиторий на чтение <see cref="UserOrganization"/>
/// </summary>
public interface IUserOrganizationReadRepository
{
    /// <summary>
    /// Получает <see cref="UserOrganization"/> по идентификаторам пользователя и организации
    /// </summary>
    Task<UserOrganization?> GetByUserAndOrganizationIdAsync(Guid userId, Guid organizationId, CancellationToken cancellationToken);

    /// <summary>
    /// Получает список <see cref="UserOrganization"/> по идентификатору организации
    /// </summary>
    Task<IReadOnlyCollection<UserOrganization>> GetByOrganizationIdAsync(Guid organizationId, CancellationToken cancellationToken);
}
