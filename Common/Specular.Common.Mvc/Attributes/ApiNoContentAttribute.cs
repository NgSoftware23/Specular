using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace NgSoftware.Specular.Common.Mvc.Attributes;

/// <summary>
/// Фильтр, который определяет тип значения и код состояния 204, возвращаемый действием
/// </summary>
public class ApiNoContentAttribute : ProducesResponseTypeAttribute
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="ApiNoContentAttribute"/>
    /// </summary>
    public ApiNoContentAttribute()
        : base((int)HttpStatusCode.NoContent)
    {
    }
}
