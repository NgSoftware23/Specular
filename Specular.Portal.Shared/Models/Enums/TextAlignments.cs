using NgSoftware.Specular.Portal.Shared.Infrastructires;

namespace NgSoftware.Specular.Portal.Shared.Models.Enums;

/// <summary>
/// Выравнивание текста
/// </summary>
public enum TextAlignments
{
    /// <summary>По левому краю</summary>
    [CssClass("text-left")]
    Left,

    /// <summary>По центру</summary>
    [CssClass("text-center")]
    Center,

    /// <summary>По правому краю</summary>
    [CssClass("text-right")]
    Right,

    /// <summary>По ширине</summary>
    [CssClass("text-justify")]
    Justify,
}
