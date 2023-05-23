namespace NgSoftware.Specular.Administrations.Api.Resources;

/// <summary>
/// Общие константы для администрирования
/// </summary>
public class AccountConstants
{
    /// <summary>
    /// Префикс имени документации
    /// </summary>
    public const string DocPrefix = "account";

    /// <summary>
    /// Заголовок документации
    /// </summary>
    public const string DocName = "Account";

    /// <summary>
    /// Роут контроллера с версионированием по умолчанию
    /// </summary>
    public const string DefaultControllerRoute = "v{version:apiVersion}/[controller]";
}
