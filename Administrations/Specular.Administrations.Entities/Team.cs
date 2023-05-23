using NgSoftware.Specular.Context.Entities.Contracts.Models;

namespace NgSoftware.Specular.Administrations.Entities;

/// <summary>
/// Команда
/// </summary>
public class Team : BaseAuditEntity
{
    /// <summary>
    /// Имя
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Значение <see cref="Name"/> в нижнем регистре
    /// </summary>
    public string NameLowerCase { get; set; } = string.Empty;

    /// <summary>
    /// Описание
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Идентификатор организации
    /// </summary>
    public Guid OrganizationId { get; set; }

    /// <summary>
    /// Навигационное свойство организации
    /// </summary>
    public virtual Organization? Organization { get; set; }

    /// <summary>
    /// Пользователи команды
    /// </summary>
    public ICollection<UserTeam> Users { get; set; }

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="Team"/>
    /// </summary>
    public Team()
    {
        Users = new List<UserTeam>();
    }
}
