using NgSoftware.Specular.Context.Entities.Contracts.Interfaces;

namespace NgSoftware.Specular.Context.Contracts;

/// <summary>
/// Интерфейс получение записей из контекста
/// </summary>
public interface IReader
{
    /// <summary>
    /// Предоставляет функциональные возможности для выполнения запросов
    /// </summary>
    IQueryable<TEntity> Read<TEntity>() where TEntity : class, IEntity;
}
