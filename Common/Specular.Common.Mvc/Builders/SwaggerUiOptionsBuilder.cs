using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using NgSoftware.Specular.Common.Mvc.Extensions;
using NgSoftware.Specular.Common.Mvc.Models;
using NgSoftware.Specular.Common.Mvc.Resources;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace NgSoftware.Specular.Common.Mvc.Builders;

/// <summary>
/// Построитель конечных точек для Swagger JSON
/// </summary>
public class SwaggerUiOptionsBuilder
{
    private readonly Action<SwaggerUiBuilderConfiguration> configurationAction;
    private readonly SwaggerUIOptions options;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="SwaggerUiOptionsBuilder"/>
    /// </summary>
    private SwaggerUiOptionsBuilder(SwaggerUIOptions options,
        Action<SwaggerUiBuilderConfiguration> configurationAction)
    {
        this.options = options;
        this.configurationAction = configurationAction;
    }

    /// <summary>
    /// Создаёт <see cref="SwaggerUiOptionsBuilder"/> с указанием конфигурации
    /// </summary>
    public static SwaggerUiOptionsBuilder Create(SwaggerUIOptions options,
        Action<SwaggerUiBuilderConfiguration> configurationAction)
        => new(options, configurationAction);

    /// <summary>
    /// Строит конечные точки для Swagger JSON
    /// </summary>
    public void Build()
    {
        var config = new SwaggerUiBuilderConfiguration();
        configurationAction?.Invoke(config);
        var controllerGroupNames = config.TargetAssembly.GetApiExplorerSettingsGroupName();
        foreach (var description in config.ApiVersionDescriptionProvider?.ApiVersionDescriptions.Reverse() ?? Array.Empty<ApiVersionDescription>())
        {
            var groupName = $"{config.DocPrefix}{description.GroupName}";
            if (groupName == CommonMvcConsts.DefaultGroupName || controllerGroupNames.Contains(groupName))
            {
                options.SwaggerEndpoint($"swagger/{groupName}/swagger.json",
                    $"{config.DocName}: {description.GroupName.ToUpperInvariant()}");
            }
        }
    }
}
