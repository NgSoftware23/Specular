using NgSoftware.Specular.Portal.Shared.Infrastructires;

namespace NgSoftware.Specular.Portal.Shared.Models.Enums;

/// <summary>
/// Выравнивание флекс-элементов вдоль главной оси контейнера
/// </summary>
public enum FlexAligns
{
    /// <summary>По левому краю или верху</summary>
    [CssClass("align-items-start")]
    Left,

    /// <summary>По центру</summary>
    [CssClass("align-items-center")]
    Center,

    /// <summary>По правому краю или низу</summary>
    [CssClass("align-items-end")]
    Right,

    /// <summary>По базовой линии</summary>
    [CssClass("align-items-baseline")]
    Baseline,

    /// <summary>По ширине</summary>
    [CssClass("align-items-stretch")]
    Stretch,
}
