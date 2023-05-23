namespace NgSoftware.Specular.Administrations.Services.Contracts.Models.User;

/// <summary>
/// Модель блокировки пользователя
/// </summary>
public class BlockUserModel
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Заблокирован до
    /// </summary>
    public DateTimeOffset BlockedAt { get; set; }

    /// <summary>
    /// Информация о блокировании
    /// </summary>
    public string Message { get; set; } = string.Empty;
}
