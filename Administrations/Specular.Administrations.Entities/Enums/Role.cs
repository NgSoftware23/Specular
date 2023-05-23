namespace NgSoftware.Specular.Administrations.Entities.Enums;

/// <summary>
/// Роли пользователя
/// </summary>
public enum Role
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
