using System.Collections.ObjectModel;
using Microsoft.IdentityModel.Tokens;
using NgSoftware.Specular.Common.Mvc.Builders;
using NgSoftware.Specular.Common.Mvc.Models;

namespace NgSoftware.Specular.Api.Tests.Client;

/// <inheritdoc />
public class BearerTokenProvider : IBearerTokenProvider
{
    private readonly string token;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="BearerTokenProvider"/>
    /// </summary>
    public BearerTokenProvider(JwtSettingsModel jwtSettings)
    {
        var moment = DateTime.Now;
        var builderResult = SecurityTokenBuilder
            .Create(ConfigureTokenOptions(jwtSettings, moment, int.MaxValue))
            .AddPersonal(x =>
            {
                x.Login = "testLogin";
                x.Identifier = Guid.NewGuid();
                x.Name = "testName";
                x.Email = "testEmail";
                x.SecurityStamp = Guid.NewGuid().ToString();
                x.Params = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());
            })
            .Build();
        token = builderResult.Token;
    }

    bool IBearerTokenProvider.HasToken => !string.IsNullOrEmpty(token);

    string IBearerTokenProvider.Token => token;

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
}
