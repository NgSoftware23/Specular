using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NgSoftware.Specular.Administrations.Services.Contracts.Exceptions;
using NgSoftware.Specular.Common.Mvc.Implementations;
using NgSoftware.Specular.Common.Mvc.Models;

namespace NgSoftware.Specular.Administrations.Api.Infrastructures;

/// <summary>
/// Фильтр для обработки ошибок раздела администрирования
/// </summary>
public class AdministrationExceptionFilter : BaseExceptionFilter
{
    /// <inheritdoc />
    public override void OnException(ExceptionContext context)
    {
        var exception = context.Exception as AdministrationException;
        if (exception == null)
        {
            return;
        }

        switch (exception)
        {
            case AdministrationValidationException ex:
                SetDataToContext(
                    new ConflictObjectResult(new ApiValidationExceptionDetail { Errors = ex.Errors, }), context);
                break;

            case AdministrationInvalidOperationException ex:
                SetDataToContext(
                    new BadRequestObjectResult(new ApiExceptionDetail { Message = ex.Message, }) { StatusCode = StatusCodes.Status406NotAcceptable, },
                    context);
                break;

            case AdministrationNotFoundException ex:
                SetDataToContext(new NotFoundObjectResult(new ApiExceptionDetail
                {
                    Message = ex.Message,
                }), context);
                break;

            default:
                SetDataToContext(new BadRequestObjectResult(new ApiExceptionDetail
                {
                    Message = exception.Message,
                }), context);
                break;
        }
    }
}
