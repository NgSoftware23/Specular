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
    private readonly Lazy<ISpecularApiClient> specularApiClient;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="SpecularApiFixture"/>
    /// </summary>
    public SpecularApiFixture()
    {
        factory = new TestWebApplicationFactory();
        specularApiClient = new Lazy<ISpecularApiClient>(CreateSpecularApiClient);
    }

    internal SpecularContext SpecularContext => factory.Services.GetRequiredService<SpecularContext>();

    internal ISpecularApiClient SpecularApiClient => specularApiClient.Value;

    Task IAsyncLifetime.InitializeAsync()
    {
        var db = factory.Services.GetRequiredService<SpecularContext>();
        return db.Database.MigrateAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        var db = factory.Services.GetRequiredService<SpecularContext>();
        await db.Database.EnsureDeletedAsync();
        await db.Database.CloseConnectionAsync();
        await db.DisposeAsync();

        await factory.DisposeAsync();
    }

    private ISpecularApiClient CreateSpecularApiClient()
    {
        var configuration = factory.Services.GetRequiredService<IConfiguration>();
        var authSetting = configuration.GetSection(JwtSettingsModel.Key).Get<JwtSettingsModel>();
        var bearerTokenProvider = new BearerTokenProvider(authSetting);
        var client = factory.CreateClient();
        return new SpecularApiTestClient(string.Empty, client, bearerTokenProvider);
    }
}
