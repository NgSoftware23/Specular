using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NgSoftware.Specular.Administrations.Services.Contracts.Models.User;
using NgSoftware.Specular.Common.Mvc.Builders;
using NgSoftware.Specular.Common.Mvc.Models;

namespace NgSoftware.Specular.Administrations.Api.Infrastructures;

/// <summary>
/// Вспомогательный класс аутентификации
/// </summary>
public class SecurityTokenManager
{
    private const int NotPersistentSecondValue = 10800;
    private const int TemporaryTokenSecondValue = 600;
    private const string AuthorizationDecisionValue = "tmp_pwd";

    /// <summary>
    /// Создаёт токен доступа
    /// </summary>
    public static SecurityTokenBuilderResult CreateToken(JwtSettingsModel authSetting,
        [NotNull] UserLoggedModel user,
        bool withRefreshToken)
    {
        var moment = DateTime.Now;
        var personalParamDict = user.PasswordIsTemporary
            ? new Dictionary<string, string>(TemporaryClaims)
            : new Dictionary<string, string>();
        var builderResult = SecurityTokenBuilder
            .Create(ConfigureTokenOptions(authSetting, moment, user.PasswordIsTemporary
                ? TemporaryTokenSecondValue
                : withRefreshToken
                    ? authSetting.LifeTimeSec
                    : NotPersistentSecondValue))
            .AddPersonal(x =>
            {
                x.Login = user.Login;
                x.Identifier = user.Id;
                x.Name = user.Name;
                x.Email = user.Email;
                x.SecurityStamp = user.SecurityStamp;
                x.Params = new ReadOnlyDictionary<string, string>(personalParamDict);
            })
            .Build();
        return builderResult;
    }

    /// <summary>
    /// Создаёт токен доступа на основе существующих клеймов
    /// </summary>
    public static SecurityTokenBuilderResult CreateTokenByClaims(JwtSettingsModel authSetting,
        string serializedClaims)
    {
        var moment = DateTime.Now;
        var claims = JsonConvert.DeserializeObject<IEnumerable<KeyValuePair<string, string>>>(serializedClaims);
        var builderResult = SecurityTokenBuilder
            .Create(ConfigureTokenOptions(authSetting, moment, authSetting.LifeTimeSec))
            .AddClaims(claims!
                .Except(TemporaryClaims)
                .Select(x => new Claim(x.Key, x.Value)))
            .Build();

        return builderResult;
    }

    /// <summary>
    /// Получает отметку безопасности из набора клеймов
    /// </summary>
    public static string GetSecurityStamp(IEnumerable<KeyValuePair<string, string>> claims)
        => claims.FirstOrDefault(x => x.Key == SecurityTokenBuilder.SecurityStampClaimType).Value;

    private static Action<SecurityTokenOptions> ConfigureTokenOptions(JwtSettingsModel authSetting,
        DateTime moment,
        int expiresSecond)
        => opt =>
        {
            opt.Audience = authSetting.Audience;
            opt.Issuer = authSetting.Issuer;
            opt.SecretKey = Base64UrlEncoder.DecodeBytes(authSetting.SecretKeyBase64);
            opt.SignKey = Base64UrlEncoder.DecodeBytes(authSetting.SignKeyBase64);
            opt.NotBefore = moment;
            opt.Expires = moment.AddSeconds(expiresSecond);
        };

    private static IEnumerable<KeyValuePair<string, string>> TemporaryClaims
        => new[] { new KeyValuePair<string, string>(ClaimTypes.AuthorizationDecision, AuthorizationDecisionValue), };
}
