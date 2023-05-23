namespace NgSoftware.Specular.Administrations.Services.Contracts.Models.Password;

/// <summary>
/// Модель смены временного пароля
/// </summary>
public class ChangeTemporaryPasswordModel
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Отметка безопасности
    /// </summary>
    public string SecurityStamp { get; set; } = string.Empty;

    /// <summary>
    /// Новый пароль
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Пароль ещё раз
    /// </summary>
    public string PasswordAgain { get; set; } = string.Empty;
}
