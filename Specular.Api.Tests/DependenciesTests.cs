using System.Reflection;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NgSoftware.Specular.Administrations.Api.Controllers;
using Specular.Api.Tests.Helpers;
using Xunit;

namespace Specular.Api.Tests;

/// <summary>
/// Тесты зависимостей контроллеров
/// </summary>
public class DependenciesTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> factory;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="DependenciesTests"/>
    /// </summary>
    public DependenciesTests(WebApplicationFactory<Program> factory)
    {
        this.factory = factory.WithWebHostBuilder(builder => builder.ConfigureTestAppConfiguration());
    }

    /// <summary>
    /// Проверка резолва зависимостей
    /// </summary>
    [Theory]
    [MemberData(nameof(AdministrationControllerCore))]
    public void ControllerCoreShouldBeResolved(Type controller)
    {
        // Arrange
        using var scope = factory.Services.CreateScope();

        // Act
        var instance = scope.ServiceProvider.GetRequiredService(controller);

        // Assert
        instance.Should().NotBeNull();
    }

    /// <summary>
    /// Коллекция контроллеров по администрированию
    /// </summary>
    public static IEnumerable<object[]>? AdministrationControllerCore =>
        Assembly.GetAssembly(typeof(AccountController))
            ?.DefinedTypes
            .Where(type => typeof(ControllerBase).IsAssignableFrom(type))
            .Where(type => !type.IsAbstract)
            .Select(type => new[] { type });
}
