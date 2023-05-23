using System.Security.Claims;
using NgSoftware.Specular.Common.Core.Contracts;

namespace NgSoftware.Specular.Api.Infrastructures;

/// <summary>
/// Поставщик опознания личности
/// </summary>
public class ApiIdentityProvider : IIdentityProvider
{
    //private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IEnumerable<Claim> claims;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="ApiIdentityProvider"/>
    /// </summary>
    public ApiIdentityProvider(IHttpContextAccessor httpContextAccessor)
    {
        claims = httpContextAccessor?.HttpContext?.User.Claims ?? Array.Empty<Claim>();
    }

    /// <inheritdoc cref="IIdentityProvider.Name"/>
    public string Name => claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value ?? "Anonymous";

    /// <inheritdoc cref="IIdentityProvider.Id"/>
    public Guid Id => Guid.TryParse(claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value, out var value)
        ? value
        : Guid.Empty;

    /// <inheritdoc cref="IIdentityProvider.Claims"/>
    public IEnumerable<KeyValuePair<string, string>> Claims
        => claims.Select(x => new KeyValuePair<string, string>(x.Type, x.Value));
}
