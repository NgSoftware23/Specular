using FluentAssertions;
using NgSoftware.Specular.Api.Client;
using Xunit;
using NgSoftware.Specular.Api.Tests.Infrastructures;
using NgSoftware.Specular.Context;

namespace NgSoftware.Specular.Api.Tests.Controllers.Administrations;

/// <summary>
/// Тесты сценариев пополнений
/// </summary>
[Collection(nameof(SpecularApiTestCollection))]
public class OrganizationControllerTests
{
    private readonly ISpecularApiClient apiClient;
    private readonly SpecularContext context;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="OrganizationControllerTests"/>
    /// </summary>
    public OrganizationControllerTests(SpecularApiFixture fixture)
    {
        context = fixture.SpecularContext;
        apiClient = fixture.CreateApiClient(fixture.TestPersonalOptions);
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
