using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using NgSoftware.Specular.Administrations.Api.Controllers;
using NgSoftware.Specular.Administrations.Api.Resources;
using NgSoftware.Specular.Common.Mvc.Extensions;
using NgSoftware.Specular.Common.Mvc.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace NgSoftware.Specular.Administrations.Api.Infrastructures;

/// <summary>
/// Расширение документации для сваггера
/// </summary>
public static class DocumentationExtensions
{
    /// <summary>
    /// Определяет один или несколько документов, которые будут созданы
    /// генератором Swagger для работы с учётными записями
    /// </summary>
    public static void SwaggerDocAccount(this SwaggerGenOptions swaggerGenOptions,
        IApiVersionDescriptionProvider provider)
        => swaggerGenOptions.BuildSwaggerDoc(GetBuilderConfiguration(provider)).Build();

    /// <summary>
    /// Добавляет swagger json endpoint для работы с учётными записями
    /// </summary>
    public static void SwaggerEndpointAccount(this SwaggerUIOptions options,
        IApiVersionDescriptionProvider provider)
        => options.BuildSwaggerEndpoint(GetBuilderConfiguration(provider)).Build();

    private static SwaggerBuilderConfiguration GetBuilderConfiguration(IApiVersionDescriptionProvider provider)
        => new()
        {
            ApiVersionDescriptionProvider = provider,
            TargetAssembly = Assembly.GetAssembly(typeof(AccountController)),
            DocName = AccountConstants.DocName,
            DocPrefix = AccountConstants.DocPrefix,
            Description = "API по работе с учётными записями",
        };
}
