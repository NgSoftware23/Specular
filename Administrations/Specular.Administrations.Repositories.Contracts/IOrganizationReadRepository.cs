using NgSoftware.Specular.Administrations.Entities;

namespace NgSoftware.Specular.Administrations.Repositories.Contracts;

/// <summary>
/// Репозиторий на чтение <see cref="Organization"/>
/// </summary>
public interface IOrganizationReadRepository
{
    /// <summary>
    /// Получает активную <see cref="Organization"/> по идентификатору
    /// </summary>
    Task<Organization?> GetActiveByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Получает активную <see cref="Organization"/> по имени
    /// </summary>
    Task<Organization?> GetActiveByNameAsync(string name, CancellationToken cancellationToken);

    /// <summary>
    /// Получает список всех организаций пользователя
    /// </summary>
    Task<IReadOnlyCollection<Organization>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Проверяет, существует ли организация с указанным именем
    /// </summary>
    Task<bool> IsActiveNameExistsAsync(string organizationName, CancellationToken cancellationToken);
}
