namespace NgSoftware.Specular.Administrations.Services.Contracts.Models.Organizations;

/// <summary>
/// Модель удаления организации
/// </summary>
public class DeleteOrganizationModel
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Идентификатор организации
    /// </summary>
    public Guid OrganizationId { get; set; }
}
