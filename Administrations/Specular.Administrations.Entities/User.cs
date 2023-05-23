using NgSoftware.Specular.Context.Entities.Contracts.Models;

namespace NgSoftware.Specular.Administrations.Entities;

/// <summary>
/// Пользователь системы
/// </summary>
public class User : BaseAuditEntity
{
    /// <summary>
    /// Имя входа
    /// </summary>
    public string Login { get; set; } = string.Empty;

    /// <summary>
    /// Значение <see cref="Login"/> в нижнем регистре
    /// </summary>
    public string LoginLowerCase { get; set; } = string.Empty;

    /// <summary>
    /// Хеш пароля
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Соль пароля
    /// </summary>
    public string PasswordSalt { get; set; } = string.Empty;

    /// <summary>
    /// Признак временного пароля
    /// </summary>
    public bool PasswordIsTemporary { get; set; }

    /// <summary>
    /// Отметка безопасности
    /// </summary>
    public string SecurityStamp { get; set; } = string.Empty;

    /// <summary>
    /// Имя
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Электронный адрес
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Значение <see cref="Email"/> в нижнем регистре
    /// </summary>
    public string EmailLowerCase { get; set; } = string.Empty;

    /// <summary>
    /// Подтверждение электронного адреса
    /// </summary>
    public bool EmailConfirmed { get; set; }

    /// <summary>
    /// Количество провальных попыток авторизации
    /// </summary>
    public int LoginAttemptFailedCount { get; set; }

    /// <summary>
    /// Признак заблокированной учётной записи
    /// </summary>
    public bool Blocked { get; set; }

    /// <summary>
    /// Время действия блокировки
    /// </summary>
    public DateTimeOffset? BlockedAt { get; set; }

    /// <summary>
    /// Заметка о пользователе
    /// </summary>
    public string Note { get; set; } = string.Empty;

    /// <summary>
    /// Организации пользователя
    /// </summary>
    public ICollection<UserOrganization> Organizations { get; set; }

    /// <summary>
    /// Приглашения пользователя
    /// </summary>
    public ICollection<UserInvite> Invites { get; set; }

    /// <summary>
    /// Команды пользователя
    /// </summary>
    public ICollection<UserTeam> Teams { get; set; }

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="User"/>
    /// </summary>
    public User()
    {
        Organizations = new List<UserOrganization>();
        Invites = new List<UserInvite>();
        Teams = new List<UserTeam>();
    }
}
