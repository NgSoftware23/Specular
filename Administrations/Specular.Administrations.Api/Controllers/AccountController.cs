using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NgSoftware.Specular.Administrations.Api.Infrastructures;
using NgSoftware.Specular.Administrations.Api.Models.Users;
using NgSoftware.Specular.Administrations.Api.Resources;
using NgSoftware.Specular.Administrations.Services.Contracts.Interfaces;
using NgSoftware.Specular.Administrations.Services.Contracts.Models.Token;
using NgSoftware.Specular.Administrations.Services.Contracts.Models.User;
using NgSoftware.Specular.Common.Core.Contracts;
using NgSoftware.Specular.Common.Mvc.Attributes;
using NgSoftware.Specular.Common.Mvc.Models;

namespace NgSoftware.Specular.Administrations.Api.Controllers;

/// <summary>
/// Работа с учётными записями
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = $"{AccountConstants.DocPrefix}v1")]
[Route(AccountConstants.DefaultControllerRoute)]
public class AccountController : ControllerBase
{
    private readonly IUserManager userManager;
    private readonly IAdministrationValidateService administrationValidateService;
    private readonly IRefreshTokenManager refreshTokenManager;
    private readonly IIdentityProvider identityProvider;
    private readonly IConfiguration configuration;
    private readonly IMapper mapper;
    private readonly IDataProtectionProvider protectionProvider;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AccountController"/>
    /// </summary>
    public AccountController(IUserManager userManager,
        IAdministrationValidateService administrationValidateService,
        IRefreshTokenManager refreshTokenManager,
        IIdentityProvider identityProvider,
        IConfiguration configuration,
        IMapper mapper,
        IDataProtectionProvider protectionProvider)
    {
        this.userManager = userManager;
        this.administrationValidateService = administrationValidateService;
        this.refreshTokenManager = refreshTokenManager;
        this.identityProvider = identityProvider;
        this.configuration = configuration;
        this.mapper = mapper;
        this.protectionProvider = protectionProvider;
    }

    /// <summary>
    /// Создаёт нового пользователя
    /// </summary>
    [AllowAnonymous]
    [HttpPost("register")]
    [ApiNoContent]
    [ApiNotAcceptable]
    [ApiConflict]
    public async Task<IActionResult> CreateUser(CreateUserApiModel modelRequest, CancellationToken cancellationToken)
    {
        var model = mapper.Map<CreateUserModel>(modelRequest);
        await administrationValidateService.ValidateAsync(model, cancellationToken);
        await userManager.CreateUserAsync(model, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Авторизация пользователя
    /// </summary>
    [AllowAnonymous]
    [HttpPost("login")]
    [ApiOk(typeof(LoginApiResponse))]
    [ApiNotFound]
    [ApiConflict]
    public async Task<IActionResult> Login(LoginApiRequest request, CancellationToken cancellationToken)
    {
        var model = new LoginModel { Login = request.Login, Password = request.Password, };
        await administrationValidateService.ValidateAsync(model, cancellationToken);
        var user = await userManager.GetActiveByLoginAndPasswordAsync(model, cancellationToken);
        var authSetting = configuration.GetSection(JwtSettingsModel.Key).Get<JwtSettingsModel>();
        var securityTokenBuilder = SecurityTokenManager.CreateToken(authSetting, user, request.IsRemember);
        var result = new LoginApiResponse { Token = securityTokenBuilder.Token, RefreshToken = string.Empty, };
        if (request.IsRemember)
        {
            var refreshTokenId = await refreshTokenManager.CreateRefreshTokenAsync(new CreateRefreshTokenModel
            {
                UserId = user.Id,
                SecurityStamp = user.SecurityStamp,
                ExpiredDays = authSetting.RefreshLifeTimeDays,
                AccessPayload = JsonConvert.SerializeObject(securityTokenBuilder.Claims
                    .Select(x => new KeyValuePair<string, string>(x.Type, x.Value))),
            }, cancellationToken);
            var protector = protectionProvider.CreateProtector(typeof(AccountController).ToString());
            result.RefreshToken = Base64UrlEncoder.Encode(protector.Protect(refreshTokenId.ToByteArray()));
        }

        return Ok(result);
    }

    /// <summary>
    /// Обновить токен доступа
    /// </summary>
    [AllowAnonymous]
    [HttpPost("refresh")]
    [ApiOk(typeof(LoginApiResponse))]
    [ApiNotFound]
    public async Task<IActionResult> Refresh(RefreshTokenApiRequest request, CancellationToken cancellationToken)
    {
        var protector = protectionProvider.CreateProtector(typeof(AccountController).ToString());
        var value = protector.Unprotect(Base64UrlEncoder.DecodeBytes(request.RefreshToken));

        var model = await refreshTokenManager.UpdateRefreshTokenAsync(new Guid(value), cancellationToken);
        var authSetting = configuration.GetSection(JwtSettingsModel.Key).Get<JwtSettingsModel>();
        var securityTokenBuilder = SecurityTokenManager.CreateTokenByClaims(authSetting, model.ClaimPayload);
        var refreshToken = Base64UrlEncoder.Encode(protector.Protect(model.TokenId.ToByteArray()));
        var result = new LoginApiResponse
        {
            Token = securityTokenBuilder.Token,
            RefreshToken = refreshToken,
        };

        return Ok(result);
    }

        /// <summary>
    /// Выход из системы
    /// </summary>
    [Authorize]
    [HttpPut("logoff")]
    [ApiNoContent]
    [ApiUnauthorized]
    public async Task<IActionResult> Logoff(CancellationToken cancellationToken)
    {
        var userId = identityProvider.Id;
        await refreshTokenManager.DeleteRefreshTokenByUserIdAsync(userId, cancellationToken);
        return NoContent();
    }
}
