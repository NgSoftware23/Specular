namespace NgSoftware.Specular.Portal.Shared.Infrastructires;

/// <summary>
/// Методы расширения
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Получает значение css класса из перечисления
    /// </summary>
    public static string ToCssClass(this Enum value)
    {
        var type = value.GetType();
        var memInfo = type.GetMember(value.ToString());
        var attributes = memInfo[0].GetCustomAttributes(typeof(CssClassAttribute), false);

        return attributes.OfType<CssClassAttribute>()
                   .Select(x => x.ClassName)
                   .First()
               ?? string.Empty;
    }
}
