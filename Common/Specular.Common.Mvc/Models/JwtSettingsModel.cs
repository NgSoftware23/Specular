namespace NgSoftware.Specular.Common.Mvc.Models;

/// <summary>
/// Authentication settings model
/// </summary>
public class JwtSettingsModel
{
    /// <summary>
    /// Settings key name
    /// </summary>
    public const string Key = "AuthJwtSettings";

    /// <summary>
    /// Issuer
    /// </summary>
    public string? Issuer { get; set; }

    /// <summary>
    /// Audience
    /// </summary>
    public string? Audience { get; set; }

    /// <summary>
    /// Key uses in hash function like SHA256
    /// </summary>
    public string? SignKeyBase64 { get; set; }

    /// <summary>
    /// 256 bits key uses for symmetric cryptography
    /// </summary>
    public string? SecretKeyBase64 { get; set; }

    /// <summary>
    /// Token life time in seconds
    /// </summary>
    public int LifeTimeSec { get; set; }

    /// <summary>
    /// Clock skew in seconds
    /// </summary>
    public int ClockSkewSec { get; set; }

    /// <summary>
    /// Время жизни токена обновления в днях
    /// </summary>
    public int RefreshLifeTimeDays { get; set; }
}
