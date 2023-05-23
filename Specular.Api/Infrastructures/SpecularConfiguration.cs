namespace NgSoftware.Specular.Api.Infrastructures;

/// <summary>
/// Конфигурирование проекта
/// </summary>
public sealed partial class SpecularConfiguration
{
    private readonly IConfiguration configuration;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="SpecularConfiguration"/>
    /// </summary>
    public SpecularConfiguration(IConfiguration configuration)
    {
        this.configuration = configuration;
    }
}
