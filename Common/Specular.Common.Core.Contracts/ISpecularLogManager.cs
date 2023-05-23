namespace NgSoftware.Specular.Common.Core.Contracts;

/// <summary>
/// Менеджер по созданию логгера
/// </summary>
public interface ISpecularLogManager
{
    /// <summary>
    /// Создаёт логгер для указанного типа
    /// </summary>
    ISpecularLogger GetLogger<T>();

    /// <summary>
    /// Создаёт логгер для указанноого типа и переданными тегами
    /// </summary>
    ISpecularLogger GetLogger<T>(params string[] tags);
}
