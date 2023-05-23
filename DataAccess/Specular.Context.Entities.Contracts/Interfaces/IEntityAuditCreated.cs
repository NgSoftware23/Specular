namespace NgSoftware.Specular.Context.Entities.Contracts.Interfaces;

/// <summary>
/// Аудит создания сущностей
/// </summary>
public interface IEntityAuditCreated
{
    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Кто создал
    /// </summary>
    public string CreatedBy { get; set; }
}
