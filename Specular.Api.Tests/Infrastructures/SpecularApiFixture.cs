using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NgSoftware.Specular.Api.Client;
using NgSoftware.Specular.Api.Tests.Client;
using NgSoftware.Specular.Common.Mvc.Models;
using NgSoftware.Specular.Context;
using Xunit;

namespace NgSoftware.Specular.Api.Tests.Infrastructures;

/// <summary>
/// Фикстура для поднятия апишки Specular
/// </summary>
public class SpecularApiFixture : IAsyncLifetime
{
    private readonly TestWebApplicationFactory factory;
    private SpecularContext? specularContext;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="SpecularApiFixture"/>
    /// </summary>
    public SpecularApiFixture()
    {
        factory = new TestWebApplicationFactory();
    }

    internal SpecularContext SpecularContext
    {
        get
        {
            if (specularContext != null)
            {
                return specularContext;
            }

            var scope = factory.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            specularContext = scope.ServiceProvider.GetRequiredService<SpecularContext>();
            return specularContext;
        }
    }

    internal PersonalOptions CreateTestPersonalOptions()
        => new()
        {
            Identifier = Guid.NewGuid(),
            Email = $"Email{Guid.NewGuid():N}",
            Login = $"Login{Guid.NewGuid():N}",
            Name = $"Name{Guid.NewGuid():N}",
            SecurityStamp = Guid.NewGuid().ToString(),
            Params = new Dictionary<string, string>(),
        };

    internal ISpecularApiClient CreateAnonymousApiClient() => new SpecularApiTestClient(string.Empty, factory.CreateClient(), null);

    internal ISpecularApiClient CreateApiClient(PersonalOptions options) => CreateSpecularApiClient(options);

    Task IAsyncLifetime.InitializeAsync() => SpecularContext.Database.MigrateAsync();

    async Task IAsyncLifetime.DisposeAsync()
    {
        await SpecularContext.Database.EnsureDeletedAsync();
        await SpecularContext.Database.CloseConnectionAsync();
        await SpecularContext.DisposeAsync();
        await factory.DisposeAsync();
    }

    private ISpecularApiClient CreateSpecularApiClient(PersonalOptions options)
    {
        var configuration = factory.Services.GetRequiredService<IConfiguration>();
        var authSetting = configuration.GetSection(JwtSettingsModel.Key).Get<JwtSettingsModel>();
        var bearerTokenProvider = new BearerTokenProvider(authSetting, options);
        var client = factory.CreateClient();
        return new SpecularApiTestClient(string.Empty, client, bearerTokenProvider);
    }
}
