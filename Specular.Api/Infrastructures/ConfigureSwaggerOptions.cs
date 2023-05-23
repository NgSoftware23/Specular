using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using NgSoftware.Specular.Administrations.Api.Infrastructures;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NgSoftware.Specular.Api.Infrastructures;

/// <summary>
/// <inheritdoc cref="IConfigureNamedOptions{TOptions}"/>
/// </summary>
public class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider provider;

    /// <summary>
    /// Инициализирует новый эеземпляр <see cref="ConfigureSwaggerOptions"/>
    /// </summary>
    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        this.provider = provider;
    }

    /// <summary>
    /// Configure each API discovered for Swagger Documentation
    /// </summary>
    public void Configure(SwaggerGenOptions options)
    {
        options.SwaggerDocAccount(provider);
    }

    /// <summary>
    /// Configure Swagger Options. Inherited from the Interface
    /// </summary>
    public void Configure(string name, SwaggerGenOptions options)
    {
        Configure(options);
    }
}
