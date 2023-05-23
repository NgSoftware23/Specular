using NgSoftware.Specular.Common.Mvc.Builders;
using NgSoftware.Specular.Common.Mvc.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NgSoftware.Specular.Common.Mvc.Extensions;

/// <summary>
/// Методы расширения для <see cref="SwaggerGenOptions"/>
/// </summary>
public static class SwaggerGenOptionsExtensions
{
    /// <summary>
    /// Определяет один или несколько документов, которые будут созданы генератором Swagger
    /// </summary>
    public static SwaggerGenOptionsBuilder BuildSwaggerDoc(this SwaggerGenOptions swaggerGenOptions,
        SwaggerBuilderConfiguration configuration)
        => SwaggerGenOptionsBuilder.Create(swaggerGenOptions, x =>
        {
            x.Description = configuration.Description;
            x.ApiVersionDescriptionProvider = configuration.ApiVersionDescriptionProvider;
            x.DocPrefix = configuration.DocPrefix;
            x.DocName = configuration.DocName;
            x.TargetAssembly = configuration.TargetAssembly;
        });
}
