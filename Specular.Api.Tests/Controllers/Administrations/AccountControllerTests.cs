using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NgSoftware.Specular.Administrations.Entities;
using NgSoftware.Specular.Api.Client;
using NgSoftware.Specular.Api.Tests.Infrastructures;
using NgSoftware.Specular.Context;
using Xunit;

namespace NgSoftware.Specular.Api.Tests.Controllers.Administrations;

/// <summary>
/// Тесты сценариев работы с пользователем
/// </summary>
[Collection(nameof(SpecularApiTestCollection))]
public class AccountControllerTests
{
    private readonly ISpecularApiClient apiClient;
    private readonly SpecularContext context;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AccountControllerTests"/>
    /// </summary>
    public AccountControllerTests(SpecularApiFixture fixture)
    {
        context = fixture.SpecularContext;
        apiClient = fixture.CreateAnonymousApiClient();
    }

    /// <summary>
    /// Создаёт нового пользователя
    /// </summary>
    [Fact]
    public async Task CreateUserShouldWork()
    {
        // Arrange
        var registerModel = new CreateUserApiModel
        {
            Name = $"Name{Guid.NewGuid():N}",
            Email = $"Email{Guid.NewGuid():N}@server.domain",
            Login = $"Login{Guid.NewGuid():N}",
            Password = "Qwerty123456!",
            PasswordAgain = "Qwerty123456!",
        };

        // Act
        await apiClient.AccountCreateUserAsync(registerModel);

        // Assert
        var user = await context.Set<User>().FirstOrDefaultAsync(x => x.Login == registerModel.Login);
        user.Should()
            .NotBeNull()
            .And.BeEquivalentTo(new
            {
                registerModel.Name,
                registerModel.Email,
                EmailConfirmed = true,
                Blocked = false,
                LoginLowerCase = registerModel.Login.ToLower(),
                EmailLowerCase = registerModel.Email.ToLower(),
                PasswordIsTemporary = false,
            });
    }

    /// <summary>
    /// Создаёт нового пользователя и авторизуется под ним
    /// </summary>
    [Fact]
    public async Task CreateUserAndLoginShouldWork()
    {
        // Arrange
        var password = Guid.NewGuid().ToString();
        var registerModel = new CreateUserApiModel
        {
            Name = $"Name{Guid.NewGuid():N}",
            Email = $"Email{Guid.NewGuid():N}@server.domain",
            Login = $"Login{Guid.NewGuid():N}",
            Password = password,
            PasswordAgain = password,
        };
        var loginModel = new LoginApiRequest { Login = registerModel.Login, Password = password, IsRemember = false, };

        // Act
        await apiClient.AccountCreateUserAsync(registerModel);
        var tokenResult = await apiClient.AccountLoginAsync(loginModel);

        // Assert
        tokenResult.Should().NotBeNull();
        tokenResult.Token.Should().NotBeEmpty();
        tokenResult.RefreshToken.Should().BeEmpty();
    }

    /// <summary>
    /// Создаёт нового пользователя и авторизуется под ним с перевыпуском рефреш-токена
    /// </summary>
    [Fact]
    public async Task CreateUserAndRefreshShouldWork()
    {
        // Arrange
        var password = Guid.NewGuid().ToString();
        var registerModel = new CreateUserApiModel
        {
            Name = $"Name{Guid.NewGuid():N}",
            Email = $"Email{Guid.NewGuid():N}@server.domain",
            Login = $"Login{Guid.NewGuid():N}",
            Password = password,
            PasswordAgain = password,
        };
        var loginModel = new LoginApiRequest { Login = registerModel.Login, Password = password, IsRemember = true, };

        // Act
        await apiClient.AccountCreateUserAsync(registerModel);
        var tokenResult = await apiClient.AccountLoginAsync(loginModel);
        var refreshModel = new RefreshTokenApiRequest { RefreshToken = tokenResult.RefreshToken, };
        var newToken = await apiClient.AccountRefreshAsync(refreshModel);

        // Assert
        tokenResult.Should().NotBeNull();
        tokenResult.Token.Should().NotBeEmpty();
        tokenResult.RefreshToken.Should().NotBeEmpty();

        newToken.Should().NotBeNull();
        newToken.Token.Should().NotBeEmpty();
        newToken.RefreshToken.Should().NotBeEmpty();

        newToken.Token.Should().Be(tokenResult.Token);
        newToken.RefreshToken.Should().NotBe(tokenResult.RefreshToken);
    }
}
