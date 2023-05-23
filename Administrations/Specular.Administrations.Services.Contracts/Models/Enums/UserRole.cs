namespace NgSoftware.Specular.Administrations.Services.Contracts.Models.Enums;

/// <summary>
/// Роли пользователя
/// </summary>
public enum UserRole
{
    /// <summary>
    /// Участие в сессии
    /// </summary>
    Participant = 0,

    /// <summary>
    /// Команды
    /// </summary>
    ScrumMaster = 1,

    /// <summary>
    /// Проведение сессий
    /// </summary>
    Header = 2,

    /// <summary>
    /// Учётные записи, организации, интеграции
    /// </summary>
    Admin = 3,
}
