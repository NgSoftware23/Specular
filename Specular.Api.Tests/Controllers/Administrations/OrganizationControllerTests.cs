using FluentAssertions;
using NgSoftware.Specular.Api.Client;
using Xunit;
using NgSoftware.Specular.Api.Tests.Infrastructures;
using NgSoftware.Specular.Common.Mvc.Models;
using NgSoftware.Specular.Context;

namespace NgSoftware.Specular.Api.Tests.Controllers.Administrations;

/// <summary>
/// Тесты сценариев пополнений
/// </summary>
[Collection(nameof(SpecularApiTestCollection))]
public class OrganizationControllerTests
{
    private readonly ISpecularApiClient apiClient;
    private readonly PersonalOptions options;
    private readonly SpecularContext context;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="OrganizationControllerTests"/>
    /// </summary>
    public OrganizationControllerTests(SpecularApiFixture fixture)
    {
        context = fixture.SpecularContext;
        options = new PersonalOptions
        {
            Identifier = Guid.NewGuid(),
            Email = $"Email{Guid.NewGuid():N}",
            Login = $"Login{Guid.NewGuid():N}",
            Name = $"Name{Guid.NewGuid():N}",
            SecurityStamp = $"SecurityStamp{Guid.NewGuid():N}",
            Params = new Dictionary<string, string>(),
        };
        apiClient = fixture.CreateApiClient(options);
    }

    /// <summary>
    /// Получает пустой список организаций
    /// </summary>
    [Fact]
    public async Task OrganizationGetShouldReturnEmpty()
    {
        // Act
        var result = await apiClient.OrganizationGetAsync(CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }
}
