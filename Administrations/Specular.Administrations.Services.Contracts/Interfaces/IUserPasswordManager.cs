using NgSoftware.Specular.Administrations.Services.Contracts.Models.Password;
using NgSoftware.Specular.Administrations.Services.Contracts.Models.User;

namespace NgSoftware.Specular.Administrations.Services.Contracts.Interfaces;

/// <summary>
/// Управление паролями пользователя
/// </summary>
public interface IUserPasswordManager
{
    /// <summary>
    /// Изменяет текущий пароль пользователя
    /// </summary>
    Task<UserLoggedModel> ChangePasswordAsync(ChangePasswordModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Смена пароля пользователя администратором
    /// </summary>
    Task SetTemporaryPasswordAsync(SetTemporaryPasswordModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Заменяет временный пароль на постоянный
    /// </summary>
    Task<UserLoggedModel> ChangeTemporaryPasswordAsync(ChangeTemporaryPasswordModel model, CancellationToken cancellationToken);
}
