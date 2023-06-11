using NgSoftware.Specular.Administrations.Services.Contracts.Models.Enums;

namespace NgSoftware.Specular.Administrations.Services.Contracts.Models.Teams;

/// <summary>
/// Модель команды
/// </summary>
public class TeamModel
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Имя
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Описание
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Роль пользователя
    /// </summary>
    public UserRole Role { get; set; }
}
