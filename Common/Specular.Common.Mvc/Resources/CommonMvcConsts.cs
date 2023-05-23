﻿using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace NgSoftware.Specular.Common.Mvc.Resources;

/// <summary>
/// Константы для проектов MVC
/// </summary>
public class CommonMvcConsts
{
    /// <summary>
    /// Имя группы контроллеров по умолчанию
    /// </summary>
    public const string DefaultGroupName = $"specularv1";

    /// <summary>
    /// Текст сообщения для <see cref="ApiVersionDescription.IsDeprecated"/>
    /// </summary>
    public const string ApiVersionDeprecatedMessage = " This API version has been deprecated. Please use one of the new APIs available from the explorer.";

    /// <summary>
    /// Наименование HTTP заголовка для хранения <see cref="RequestIdName"/>
    /// </summary>
    public const string RequestIdHeaderName = "Trace-Request-Id";

    /// <summary>
    /// Наименование параметра идентификатора запроса в логгере
    /// </summary>
    public const string RequestIdName = "RequestId";

    /// <summary>
    /// Наименование параметра метода выполнения в логгере
    /// </summary>
    public const string RequestMethodName = "RequestMethod";
}
