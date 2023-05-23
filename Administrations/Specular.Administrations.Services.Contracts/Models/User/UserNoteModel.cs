namespace NgSoftware.Specular.Administrations.Services.Contracts.Models.User;

/// <summary>
/// Модель заметки о пользователе
/// </summary>
public class UserNoteModel
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Заметка о пользователе
    /// </summary>
    public string Note { get; set; } = string.Empty;
}
