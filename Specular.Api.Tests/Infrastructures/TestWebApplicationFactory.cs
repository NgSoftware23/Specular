using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NgSoftware.Specular.Api.Tests.Helpers;
using NgSoftware.Specular.Context;

namespace NgSoftware.Specular.Api.Tests.Infrastructures;

/// <summary>
/// Хост для тестирования Api
/// </summary>
public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    /// <inheritdoc cref="WebApplicationFactory{TEntryPoint}.CreateHost"/>
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment(Environments.Development);
        return base.CreateHost(builder);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestAppConfiguration();
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<SpecularContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddScoped<DbContextOptions<SpecularContext>>(options =>
            {
                var builder = new DbContextOptionsBuilder<SpecularContext>()
                    .UseInMemoryDatabase(databaseName: $"SpecularInMemoryDatabase{Guid.NewGuid():N}")
                    .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                return builder.Options;
            });
        });
    }
}
