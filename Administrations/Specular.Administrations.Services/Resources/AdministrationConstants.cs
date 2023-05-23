namespace NgSoftware.Specular.Administrations.Services.Resources;

/// <summary>
/// Общие внутренние константы
/// </summary>
public class AdministrationConstants
{
    /// <summary>
    /// Минимальная длина пароля
    /// </summary>
    public const int MinimumPasswordLength = 6;

    /// <summary>
    /// Количество попыток авторизации
    /// </summary>
    public const byte MaximumFailedCount = 3;

    /// <summary>
    /// Задержка после неудачных попыток авторизации, в минутах
    /// </summary>
    public const byte LoginAttemptMinutesDelay = 5;

    /// <summary>
    /// Текст блокирования после неудачных попыток авторизации
    /// </summary>
    public const string FailedCountMessage = "Login attempt failed count";
}
