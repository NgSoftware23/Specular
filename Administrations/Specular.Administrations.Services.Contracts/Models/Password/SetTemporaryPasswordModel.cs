namespace NgSoftware.Specular.Administrations.Services.Contracts.Models.Password;

/// <summary>
/// Модель смены пароля пользователя администратором
/// </summary>
public class SetTemporaryPasswordModel
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Новый пароль
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Пароль ещё раз
    /// </summary>
    public string PasswordAgain { get; set; } = string.Empty;
}
