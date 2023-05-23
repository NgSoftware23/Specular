namespace NgSoftware.Specular.Administrations.Services.Contracts.Models.Token;

/// <summary>
/// Модель обновления токена обновления
/// </summary>
public class UpdateRefreshTokenModel
{
    /// <summary>
    /// Идентификатор токена оюновления
    /// </summary>
    public Guid TokenId { get; set; }

    /// <summary>
    /// Полезные данные для токена доступа
    /// </summary>
    public string ClaimPayload { get; set; } = string.Empty;
}
