namespace NgSoftware.Specular.Administrations.Services.Contracts.Models.Teams;

/// <summary>
/// Модель удаления команды
/// </summary>
public class DeleteTeamModel
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }
}
