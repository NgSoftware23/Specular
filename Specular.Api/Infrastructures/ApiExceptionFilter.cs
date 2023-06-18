using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NgSoftware.Specular.Common.Mvc.Implementations;
using NgSoftware.Specular.Common.Mvc.Models;

namespace NgSoftware.Specular.Api.Infrastructures;

/// <summary>
/// Фильтр для обработки общих ошибок
/// </summary>
public class ApiExceptionFilter : BaseExceptionFilter
{
    /// <inheritdoc />
    public override void OnException(ExceptionContext context)
    {
        if(context.Exception is UnauthorizedAccessException exception)
        {
            SetDataToContext(new UnauthorizedObjectResult(new ApiExceptionDetail
            {
                Message = exception.Message,
            }), context);
        }
    }
}
