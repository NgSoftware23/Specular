namespace NgSoftware.Specular.Administrations.Services.Contracts.Models.Password;

/// <summary>
/// Модель смены пароля пользователя
/// </summary>
public class ChangePasswordModel : ChangeTemporaryPasswordModel
{
    /// <summary>
    /// Старый пароль
    /// </summary>
    public string OldPassword { get; set; } = string.Empty;
}
