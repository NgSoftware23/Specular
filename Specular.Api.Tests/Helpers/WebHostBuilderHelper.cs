using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace NgSoftware.Specular.Api.Tests.Helpers;

static internal class WebHostBuilderHelper
{
    /// <summary>
    /// Конфигурирование IWebHostBuilder
    /// </summary>
    public static void ConfigureTestAppConfiguration(this IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, config) =>
        {
            var projectDir = Directory.GetCurrentDirectory();
            var configPath = Path.Combine(projectDir, "appsettings.integration.json");
            config.AddJsonFile(configPath).AddEnvironmentVariables();
        });
    }
}
