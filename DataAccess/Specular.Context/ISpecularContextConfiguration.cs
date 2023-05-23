namespace NgSoftware.Specular.Context;

/// <summary>
/// Конфигурирование контекста
/// </summary>
public interface ISpecularContextConfiguration
{
    /// <summary>
    /// Строка подключения
    /// </summary>
    string ConnectionString { get; }
}
