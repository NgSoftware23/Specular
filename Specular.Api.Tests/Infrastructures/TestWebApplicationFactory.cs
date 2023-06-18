using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
        builder.UseEnvironment("integration");
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

            services.AddSingleton<DbContextOptions<SpecularContext>>(provider =>
            {
                var configuration = provider.GetRequiredService<ISpecularContextConfiguration>();
                var dbContextOptions = new DbContextOptions<SpecularContext>(new Dictionary<Type, IDbContextOptionsExtension>());
                var optionsBuilder = new DbContextOptionsBuilder<SpecularContext>(dbContextOptions)
                    .UseApplicationServiceProvider(provider)
                    .UseNpgsql(connectionString: string.Format(configuration.ConnectionString, Guid.NewGuid().ToString("N")));
                return optionsBuilder.Options;
            });
        });
    }
}
