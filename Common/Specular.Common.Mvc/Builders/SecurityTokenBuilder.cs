using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using NgSoftware.Specular.Common.Mvc.Models;

namespace NgSoftware.Specular.Common.Mvc.Builders;

/// <summary>
/// Построитель для <see cref="SecurityToken"/>
/// </summary>
public class SecurityTokenBuilder
{
    /// <summary>
    /// Имя клейма для хранения отметки безопасности
    /// </summary>
    public const string SecurityStampClaimType = ClaimTypes.Hash;
    private Action<PersonalOptions>? actionPersonalOptions;
    private Action<SecurityTokenOptions>? actionSecurityTokenOptions;
    private IEnumerable<Claim> personalClaim = Array.Empty<Claim>();

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="SecurityTokenBuilder"/>
    /// </summary>
    internal SecurityTokenBuilder() { }

    internal void SetActionSecurityTokenOptions(Action<SecurityTokenOptions> action)
    {
        actionSecurityTokenOptions = action;
    }

    /// <summary>
    /// Добавляет <see cref="PersonalOptions"/> для формирования <see cref="SecurityToken"/>
    /// </summary>
    public SecurityTokenBuilder AddPersonal(Action<PersonalOptions> configurePersonalOptions)
    {
        actionPersonalOptions = configurePersonalOptions;
        return this;
    }

    /// <summary>
    /// Добавляет список <see cref="Claim"/> для формирования токена
    /// </summary>
    /// <remarks>Отменяет действие <see cref="AddPersonal"/></remarks>
    public SecurityTokenBuilder AddClaims(IEnumerable<Claim> claims)
    {
        personalClaim = claims;
        return this;
    }

    /// <summary>
    /// Создаёт <see cref="SecurityTokenBuilder"/> с указанием <see cref="SecurityTokenOptions"/>
    /// </summary>
    public static SecurityTokenBuilder Create(Action<SecurityTokenOptions> configureTokenOptions)
    {
        var builder = new SecurityTokenBuilder();
        builder.SetActionSecurityTokenOptions(configureTokenOptions);
        return builder;
    }

    /// <summary>
    /// Создаёт серилизованный <see cref="SecurityToken"/>
    /// </summary>
    public SecurityTokenBuilderResult Build()
    {
        var configureOptions = new SecurityTokenOptions();
        actionSecurityTokenOptions?.Invoke(configureOptions);
        List<Claim> claims;
        if (!personalClaim.Any())
        {
            var personalOptions = new PersonalOptions();
            actionPersonalOptions?.Invoke(personalOptions);

            claims = new List<Claim>()
            {
                new (ClaimTypes.Name, personalOptions.Name),
                new (ClaimTypes.Email, personalOptions.Email),
                new (ClaimTypes.GivenName, personalOptions.Login),
                new (ClaimTypes.NameIdentifier, personalOptions.Identifier.ToString()),
                new (SecurityStampClaimType, personalOptions.SecurityStamp),
            };

            if (personalOptions.Params?.Any() == true)
            {
                claims.AddRange(personalOptions.Params.Select(x => new Claim(x.Key, x.Value)));
            }
        }
        else
        {
            claims = new List<Claim>(personalClaim);
        }

        var signCred = new SigningCredentials(new SymmetricSecurityKey(configureOptions.SignKey),
            SecurityAlgorithms.HmacSha256);

        var encCred = new EncryptingCredentials(new SymmetricSecurityKey(configureOptions.SecretKey),
            JwtConstants.DirectKeyUseAlg,
            SecurityAlgorithms.Aes256CbcHmacSha512);

        var jwtHandler = new JwtSecurityTokenHandler();
        var jwtToken = jwtHandler.CreateJwtSecurityToken(
            new SecurityTokenDescriptor()
            {
                Issuer = configureOptions.Issuer,
                Audience = configureOptions.Audience,
                NotBefore = configureOptions.NotBefore,
                Expires = configureOptions.Expires,
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = signCred,
                //EncryptingCredentials = encCred,
            });

        return new SecurityTokenBuilderResult
        {
            Token = jwtHandler.WriteToken(jwtToken),
            Claims = claims,
        };
    }
}
