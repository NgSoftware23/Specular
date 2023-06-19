using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NgSoftware.Specular.Administrations.Entities;
using NgSoftware.Specular.Api.Client;
using Xunit;
using NgSoftware.Specular.Api.Tests.Infrastructures;
using NgSoftware.Specular.Context;

namespace NgSoftware.Specular.Api.Tests.Controllers.Administrations;

/// <summary>
/// Тесты сценариев работы с организацией
/// </summary>
[Collection(nameof(SpecularApiTestCollection))]
public class OrganizationControllerTests
{
    private readonly ISpecularApiClient apiClient;
    private readonly SpecularContext context;
    private readonly Guid userId;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="OrganizationControllerTests"/>
    /// </summary>
    public OrganizationControllerTests(SpecularApiFixture fixture)
    {
        context = fixture.SpecularContext;
        var personal = fixture.CreateTestPersonalOptions();
        userId = personal.Identifier;
        apiClient = fixture.CreateApiClient(personal);
    }

    /// <summary>
    /// Получает пустой список организаций
    /// </summary>
    [Fact]
    public async Task OrganizationGetShouldReturnEmpty()
    {
        // Act
        var result = await apiClient.OrganizationGetAsync();

        // Assert
        result.Should().BeEmpty();
    }

    /// <summary>
    /// Создаёт организацию и получает её в виде списка
    /// </summary>
    [Fact]
    public async Task OrganizationCreateAndGetShouldReturnValue()
    {
        // Arrange
        await CreateUser();
        var createModel = new CreateOrganizationApiModel
        {
            Name = $"Name{Guid.NewGuid():N}",
            Description = $"Description{Guid.NewGuid():N}",
        };

        // Act
        var createResult = await apiClient.OrganizationCreateAsync(createModel);
        var getResult = await apiClient.OrganizationGetAsync();

        // Assert
        createResult.Should()
            .NotBeNull()
            .And.BeEquivalentTo(new { createModel.Name, createModel.Description, });
        getResult.Should()
            .NotBeNull()
            .And.HaveCount(1)
            .And.ContainSingle(x => x.Id == createResult.Id);
    }

    /// <summary>
    /// Создаёт организацию и редактирует её
    /// </summary>
    [Fact]
    public async Task OrganizationCreateAndUpdateShouldReturnValue()
    {
        // Arrange
        await CreateUser();
        var createModel = new CreateOrganizationApiModel
        {
            Name = $"Name{Guid.NewGuid():N}",
            Description = $"Description{Guid.NewGuid():N}",
        };
        var updateModel = new OrganizationApiModel
        {
            Name = $"Name{Guid.NewGuid():N}",
            Description = $"Description{Guid.NewGuid():N}",
        };

        // Act
        var createResult = await apiClient.OrganizationCreateAsync(createModel);
        updateModel.Id = createResult.Id;
        await apiClient.OrganizationUpdateAsync(updateModel);
        var getResult = await apiClient.OrganizationGetAsync();

        // Assert
        createResult.Should()
            .NotBeNull()
            .And.BeEquivalentTo(new { createModel.Name, createModel.Description, });
        getResult.Should()
            .NotBeNull()
            .And.HaveCount(1)
            .And.ContainSingle(x => x.Id == createResult.Id &&
                                    x.Name == updateModel.Name &&
                                    x.Description == updateModel.Description);
    }

    /// <summary>
    /// Создаёт организацию и удаляет её
    /// </summary>
    [Fact]
    public async Task OrganizationCreateAndDeleteShouldReturnValue()
    {
        // Arrange
        await CreateUser();
        var createModel = new CreateOrganizationApiModel
        {
            Name = $"Name{Guid.NewGuid():N}",
            Description = $"Description{Guid.NewGuid():N}",
        };

        // Act
        var createResult = await apiClient.OrganizationCreateAsync(createModel);
        await apiClient.OrganizationDeleteAsync(createResult.Id);
        var getResult = await apiClient.OrganizationGetAsync();

        // Assert
        createResult.Should()
            .NotBeNull()
            .And.BeEquivalentTo(new { createModel.Name, createModel.Description, });
        getResult.Should().BeEmpty();
    }

    private async Task CreateUser()
    {
        var exist = await context.Set<User>().AnyAsync(x => x.Id == userId);
        if (exist)
        {
            return;
        }
        var result = new User()
        {
            Id = userId,
            Name = $"Name{Guid.NewGuid():N}",
            CreatedBy = $"CreatedBy{Guid.NewGuid():N}",
            Email = $"Email{Guid.NewGuid():N}",
            Login = $"Login{Guid.NewGuid():N}",
            LoginLowerCase = $"LoginLowerCase{Guid.NewGuid():N}".ToLower(),
            PasswordHash = "WT0qCdXVES9q74kYj3GbQU0kyYkDFiNQA9JazziXqjM=",
            PasswordSalt = "/f0Ji7HVTA4iydlXzHux7Ex9KKIJGssQ0wc6vU0x23g=",
            SecurityStamp = $"SecurityStamp{Guid.NewGuid():N}",
            UpdatedBy = $"UpdatedBy{Guid.NewGuid():N}",
        };
        await context.AddAsync(result);
        await context.SaveChangesAsync();
    }

}
