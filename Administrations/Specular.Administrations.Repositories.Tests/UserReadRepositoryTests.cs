using FluentAssertions;
using NgSoftware.Specular.Administrations.Repositories.Contracts;
using NgSoftware.Specular.Context.Tests;
using Xunit;

namespace NgSoftware.Specular.Administrations.Repositories.Tests;

/// <summary>
/// Тесты на <see cref="IUserReadRepository"/>
/// </summary>
public class UserReadRepositoryTests : SpecularContextInMemory
{
    private readonly IUserReadRepository userReadRepository;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="UserReadRepositoryTests"/>
    /// </summary>
    public UserReadRepositoryTests()
    {
        userReadRepository = new UserReadRepository(Context);
    }

    /// <summary>
    /// Возвращает null если не находит пользователя по Id
    /// </summary>
    [Fact]
    public async Task GetByIdShouldReturnNull()
    {
        //Arrange
        var targetId = Guid.NewGuid();

        // Act
        var result = await userReadRepository.GetByIdAsync(targetId, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает пользователя по Id
    /// </summary>
    [Fact]
    public async Task GetByIdShouldReturnValue()
    {
        //Arrange
        var targetUser = TestDataGenerator.GetUser();
        await Context.AddAsync(targetUser, CancellationToken.None);
        await Context.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await userReadRepository.GetByIdAsync(targetUser.Id, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(targetUser);
    }

    /// <summary>
    /// Возвращает данные при запросе существующего логина
    /// </summary>
    [Theory]
    [MemberData(nameof(IsActiveExistsData))]
    public async Task IsActiveLoginExistsShouldReturnValue(DateTimeOffset? deletedAt, bool expected)
    {
        //Arrange
        var targetUser = TestDataGenerator.GetUser(x => x.DeletedAt = deletedAt);
        await Context.AddAsync(targetUser, CancellationToken.None);
        await Context.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await userReadRepository.IsActiveLoginExistsAsync(targetUser.Login, CancellationToken.None);

        // Assert
        result.Should().Be(expected);
    }

    /// <summary>
    /// Возвращает данные при запросе существующего логина
    /// </summary>
    [Theory]
    [MemberData(nameof(IsActiveExistsData))]
    public async Task IsActiveEmailExistsShouldReturnValue(DateTimeOffset? deletedAt, bool expected)
    {
        //Arrange
        var targetUser = TestDataGenerator.GetUser(x => x.DeletedAt = deletedAt);
        await Context.AddAsync(targetUser, CancellationToken.None);
        await Context.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await userReadRepository.IsActiveEmailExistsAsync(targetUser.Email, CancellationToken.None);

        // Assert
        result.Should().Be(expected);
    }

    /// <summary>
    /// Возвращает null если не находит активного пользователя по Id
    /// </summary>
    [Fact]
    public async Task GetActiveByIdShouldReturnNull()
    {
        //Arrange
        var targetId = Guid.NewGuid();

        // Act
        var result = await userReadRepository.GetActiveByIdAsync(targetId, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает null если не находит активного пользователя по Id
    /// т.к. пользователь удалён
    /// </summary>
    [Fact]
    public async Task GetActiveByIdShouldReturnDeletedNull()
    {
        //Arrange
        var targetUser = TestDataGenerator.GetUser(x => x.DeletedAt = DateTimeOffset.UtcNow);
        await Context.AddAsync(targetUser, CancellationToken.None);
        await Context.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await userReadRepository.GetActiveByIdAsync(targetUser.Id, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает null если не находит активного пользователя по Id
    /// т.к. пользователь заблокирован
    /// </summary>
    [Fact]
    public async Task GetActiveByIdShouldReturnBlockedNull()
    {
        //Arrange
        var targetUser = TestDataGenerator.GetUser(x => x.Blocked = true);
        await Context.AddAsync(targetUser, CancellationToken.None);
        await Context.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await userReadRepository.GetActiveByIdAsync(targetUser.Id, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает активного пользователя по Id
    /// </summary>
    [Fact]
    public async Task GetActiveByIdShouldReturnValue()
    {
        //Arrange
        var targetUser = TestDataGenerator.GetUser();
        await Context.AddAsync(targetUser, CancellationToken.None);
        await Context.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await userReadRepository.GetActiveByIdAsync(targetUser.Id, CancellationToken.None);

        // Assert
        // Assert
        result.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(targetUser);
    }

    /// <summary>
    /// Возвращает null если не находит пользователя по email
    /// </summary>
    [Fact]
    public async Task GetActiveByMailShouldReturnNull()
    {
        //Arrange
        var targetMail = "GetActiveByMailShouldReturnNull_mail";

        // Act
        var result = await userReadRepository.GetActiveByMailAsync(targetMail, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает пользователя по email
    /// </summary>
    [Fact]
    public async Task GetActiveByMailShouldReturnValue()
    {
        //Arrange
        var targetUser = TestDataGenerator.GetUser();
        await Context.AddAsync(targetUser, CancellationToken.None);
        await Context.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await userReadRepository.GetActiveByMailAsync(targetUser.Email, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(targetUser);
    }

    /// <summary>
    /// Список пользователей организации пуст
    /// </summary>
    [Fact]
    public async Task GetShouldReturnEmpty()
    {
        // Act
        var result = await userReadRepository.GetByOrganizationIdAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    /// <summary>
    /// Список пользователей содержит значения
    /// </summary>
    [Fact]
    public async Task GetShouldReturnValue()
    {
        //Arrange
        var organizationId = Guid.NewGuid();

        var targetUser1 = TestDataGenerator.GetUser();
        await Context.AddAsync(targetUser1, CancellationToken.None);
        await Context.AddAsync(TestDataGenerator.GetUserOrganization(targetUser1.Id, organizationId), CancellationToken.None);
        var targetUser2 = TestDataGenerator.GetUser();
        await Context.AddAsync(targetUser2, CancellationToken.None);
        await Context.AddAsync(TestDataGenerator.GetUserOrganization(targetUser2.Id, Guid.NewGuid()), CancellationToken.None);
        var targetUser3 = TestDataGenerator.GetUser();
        await Context.AddAsync(targetUser3, CancellationToken.None);
        await Context.AddAsync(TestDataGenerator.GetUserOrganization(targetUser3.Id, organizationId), CancellationToken.None);

        await Context.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await userReadRepository.GetByOrganizationIdAsync(organizationId, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeEmpty()
            .And
            .HaveCount(2)
            .And
            .ContainSingle(x => x.Id == targetUser1.Id)
            .And
            .ContainSingle(x => x.Id == targetUser3.Id);
    }

    /// <summary>
    /// Данные для проверки поиска в БД
    /// </summary>
    public static IEnumerable<object?[]> IsActiveExistsData()
    {
        yield return new object?[] { DateTimeOffset.MinValue, false };
        yield return new object?[] { null, true };
    }
}
