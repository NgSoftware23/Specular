using NgSoftware.Specular.Common.Core.Contracts.Models;

namespace NgSoftware.Specular.Common.Mvc.Models;

/// <summary>
/// Информация об ошибках валидации работы АПИ
/// </summary>
public class ApiValidationExceptionDetail
{
    /// <summary>
    /// Ошибки валидации
    /// </summary>
    public IEnumerable<InvalidateItemModel> Errors { get; set; } = Array.Empty<InvalidateItemModel>();
}
