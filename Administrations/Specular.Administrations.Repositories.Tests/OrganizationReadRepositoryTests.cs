using FluentAssertions;
using NgSoftware.Specular.Administrations.Repositories.Contracts;
using NgSoftware.Specular.Context.Tests;
using Xunit;

namespace NgSoftware.Specular.Administrations.Repositories.Tests;

/// <summary>
/// Тесты для <see cref="IOrganizationReadRepository"/>
/// </summary>
public class OrganizationReadRepositoryTests : SpecularContextInMemory
{
    private readonly IOrganizationReadRepository organizationReadRepository;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="OrganizationReadRepositoryTests"/>
    /// </summary>
    public OrganizationReadRepositoryTests()
    {
        organizationReadRepository = new OrganizationReadRepository(Context);
    }

    /// <summary>
    /// Возвращает null если не находит организацию по Id
    /// </summary>
    [Fact]
    public async Task GetActiveByIdShouldReturnNull()
    {
        //Arrange
        var targetId = Guid.NewGuid();

        // Act
        var result = await organizationReadRepository.GetActiveByIdAsync(targetId, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает null если не находит организацию по Id т.к. удалена
    /// </summary>
    [Fact]
    public async Task GetActiveByIdShouldReturnDeleted()
    {
        //Arrange
        var targetOrganization = TestDataGenerator.GetOrganization();
        targetOrganization.DeletedAt = DateTimeOffset.UtcNow;
        await Context.AddAsync(targetOrganization, CancellationToken.None);
        await Context.SaveChangesAsync();

        // Act
        var result = await organizationReadRepository.GetActiveByIdAsync(targetOrganization.Id, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает организацию по Id
    /// </summary>
    [Fact]
    public async Task GetActiveByIdShouldReturnValue()
    {
        //Arrange
        var targetOrganization = TestDataGenerator.GetOrganization();
        await Context.AddAsync(targetOrganization, CancellationToken.None);
        await Context.SaveChangesAsync();

        // Act
        var result = await organizationReadRepository.GetActiveByIdAsync(targetOrganization.Id, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.BeEquivalentTo(targetOrganization);
    }

    /// <summary>
    /// Возвращает пустой список организаций по Id пользователя
    /// </summary>
    [Fact]
    public async Task GetByUserIdShouldReturnNull()
    {
        //Arrange
        var targetId = Guid.NewGuid();

        // Act
        var result = await organizationReadRepository.GetByUserIdAsync(targetId, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    /// <summary>
    /// Возвращает список организаций по Id пользователя
    /// </summary>
    [Fact]
    public async Task GetByUserIdShouldReturnValue()
    {
        //Arrange
        var targetId = Guid.NewGuid();
        var organization1 = TestDataGenerator.GetOrganization();
        var organization2 = TestDataGenerator.GetOrganization();
        organization2.DeletedAt = DateTimeOffset.UtcNow;
        var organization3 = TestDataGenerator.GetOrganization();
        await Context.AddRangeAsync(organization1,
            organization2,
            organization3,
            TestDataGenerator.GetUserOrganization(targetId, organization1.Id),
            TestDataGenerator.GetUserOrganization(targetId, organization2.Id),
            TestDataGenerator.GetUserOrganization(Guid.NewGuid(), organization3.Id));
        await Context.SaveChangesAsync();

        // Act
        var result = await organizationReadRepository.GetByUserIdAsync(targetId, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeEmpty()
            .And.HaveCount(1)
            .And.ContainSingle(x => x.Id == organization1.Id);
    }

    /// <summary>
    /// Проверяет, что организация с указанным именем не существует
    /// </summary>
    [Fact]
    public async Task CheckIfOrganizationNameExistShouldReturnFalse()
    {
        //Arrange
        await Context.AddAsync(TestDataGenerator.GetOrganization());
        await Context.SaveChangesAsync();

        // Act
        var result = await organizationReadRepository.IsActiveNameExistsAsync("CheckIfOrganizationNameExistShouldReturnFalse", CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Проверяет, что организация с указанным именем не существует т.к. удалена
    /// </summary>
    [Fact]
    public async Task CheckIfOrganizationNameExistShouldReturnFalseDeleted()
    {
        //Arrange
        var targetOrganization = TestDataGenerator.GetOrganization();
        targetOrganization.DeletedAt = DateTimeOffset.UtcNow;
        await Context.AddAsync(targetOrganization);
        await Context.SaveChangesAsync();

        // Act
        var result = await organizationReadRepository.IsActiveNameExistsAsync(targetOrganization.Name, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Проверяет, что организация с указанным именем существует
    /// </summary>
    [Fact]
    public async Task CheckIfOrganizationNameExistShouldReturnTrue()
    {
        //Arrange
        var targetOrganization = TestDataGenerator.GetOrganization();
        await Context.AddAsync(targetOrganization);
        await Context.SaveChangesAsync();

        // Act
        var result = await organizationReadRepository.IsActiveNameExistsAsync(targetOrganization.Name, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Возвращает null если не находит организацию по имени
    /// </summary>
    [Fact]
    public async Task GetActiveByNameShouldReturnNull()
    {
        //Arrange
        var targetName = $"targetName{Guid.NewGuid():N}";

        // Act
        var result = await organizationReadRepository.GetActiveByNameAsync(targetName, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает null если не находит организацию по имени т.к. удалена
    /// </summary>
    [Fact]
    public async Task GetActiveByNameShouldReturnDeleted()
    {
        //Arrange
        var targetOrganization = TestDataGenerator.GetOrganization();
        targetOrganization.DeletedAt = DateTimeOffset.UtcNow;
        await Context.AddAsync(targetOrganization);
        await Context.SaveChangesAsync();

        // Act
        var result = await organizationReadRepository.GetActiveByNameAsync(targetOrganization.Name, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает организацию по имени
    /// </summary>
    [Fact]
    public async Task GetActiveByNameShouldReturnValue()
    {
        //Arrange
        var targetOrganization = TestDataGenerator.GetOrganization();
        await Context.AddAsync(targetOrganization);
        await Context.SaveChangesAsync();

        // Act
        var result = await organizationReadRepository.GetActiveByNameAsync(targetOrganization.Name, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.BeEquivalentTo(targetOrganization);
    }
}
