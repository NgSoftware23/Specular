using NgSoftware.Specular.Administrations.Services.Contracts.Models.User;

namespace NgSoftware.Specular.Administrations.Services.Contracts.Interfaces;

/// <summary>
/// Управление пользователями
/// </summary>
public interface IUserManager
{
    /// <summary>
    /// Создаёт нового пользователя
    /// </summary>
    Task CreateUserAsync(CreateUserModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Получает активного пользователя по паре логин - пароль
    /// </summary>
    Task<UserLoggedModel> GetActiveByLoginAndPasswordAsync(LoginModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Удаляет пользователя по идентификатору
    /// </summary>
    Task DeleteUserAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Блокирует пользователя
    /// </summary>
    Task BlockUserAsync(BlockUserModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Разблокирует пользователя
    /// </summary>
    Task UnBlockUserAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Получает список всех пользователей указанной организации
    /// </summary>
    Task<IEnumerable<UserInfoModel>> GetUsersByOrganizationIdAsync(Guid organizationId, CancellationToken cancellationToken);

    /// <summary>
    /// Задаёт заметку о пользователе
    /// </summary>
    Task SetNoteAsync(UserNoteModel model, CancellationToken cancellationToken);

    /// <summary>
    /// Изменяет параметры пользователя
    /// </summary>
    Task<UserLoggedModel> ModifyUserAsync(UserModifyModel model, CancellationToken cancellationToken);
}
