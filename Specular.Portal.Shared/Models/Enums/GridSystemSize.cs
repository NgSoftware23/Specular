using NgSoftware.Specular.Portal.Shared.Infrastructires;

namespace NgSoftware.Specular.Portal.Shared.Models.Enums;


/// <summary>
/// Уровни сетки, основанные на размере рабочей обрасти
/// </summary>
public enum GridSystemSize
{
    /// <summary>
    /// Размер не задан
    /// </summary>
    [CssClass("")]
    None,

    /// <summary>
    /// Размер меньше 576px
    /// </summary>
    [CssClass("xs")]
    ExtraSmall,

    /// <summary>
    /// Размер ≥576px
    /// </summary>
    [CssClass("sm")]
    Small,

    /// <summary>
    /// Размер ≥768px
    /// </summary>
    [CssClass("md")]
    Medium,

    /// <summary>
    /// Размер ≥992px
    /// </summary>
    [CssClass("lg")]
    Large,

    /// <summary>
    /// Размер ≥1200px
    /// </summary>
    [CssClass("xl")]
    ExtraLarge,

    /// <summary>
    /// Размер ≥1400px
    /// </summary>
    [CssClass("xxl")]
    ExtraExtraLarge,

    /// <summary>
    /// Текучий размер
    /// </summary>
    [CssClass("fluid")]
    Fluid,
}
