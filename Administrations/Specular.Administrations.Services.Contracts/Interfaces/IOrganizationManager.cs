using NgSoftware.Specular.Administrations.Services.Contracts.Models.Organizations;

namespace NgSoftware.Specular.Administrations.Services.Contracts.Interfaces;

/// <summary>
/// Управление организациями
/// </summary>
public interface IOrganizationManager
{
    /// <summary>
    /// Создаёт новую организацию
    /// </summary>
    Task<OrganizationModel> CreateAsync(CreateOrganizationModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Получает список организаций для указанного идентификатора пользователя
    /// </summary>
    Task<IEnumerable<OrganizationModel>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Редактирует существующую организацию
    /// </summary>
    Task<OrganizationModel> UpdateAsync(UpdateOrganizationModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Удаляет существующую организацию
    /// </summary>
    Task DeleteAsync(DeleteOrganizationModel model, CancellationToken cancellationToken);
}
