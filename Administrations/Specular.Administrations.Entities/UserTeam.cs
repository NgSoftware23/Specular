using NgSoftware.Specular.Administrations.Entities.Enums;
using NgSoftware.Specular.Context.Entities.Contracts.Models;

namespace NgSoftware.Specular.Administrations.Entities;

/// <summary>
/// Команда пользователя
/// </summary>
public class UserTeam : BaseAuditEntity
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Навигационное свойство пользователя
    /// </summary>
    public virtual User? User { get; set; }

    /// <summary>
    /// Идентификатор команды
    /// </summary>
    public Guid TeamId { get; set; }

    /// <summary>
    /// Навигационное свойство команды
    /// </summary>
    public virtual Team? Team { get; set; }

    /// <summary>
    /// Роль пользователя
    /// </summary>
    public Role Role { get; set; }
}
