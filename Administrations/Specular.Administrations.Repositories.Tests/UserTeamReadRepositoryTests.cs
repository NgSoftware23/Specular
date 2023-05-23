using FluentAssertions;
using NgSoftware.Specular.Administrations.Entities;
using NgSoftware.Specular.Administrations.Repositories.Contracts;
using NgSoftware.Specular.Context.Tests;
using Xunit;

namespace NgSoftware.Specular.Administrations.Repositories.Tests;

/// <summary>
/// Тесты на <see cref="IUserTeamReadRepository"/>
/// </summary>
public class UserTeamReadRepositoryTests : SpecularContextInMemory
{
    private readonly IUserTeamReadRepository userTeamReadRepository;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="UserTeamReadRepositoryTests"/>
    /// </summary>
    public UserTeamReadRepositoryTests()
    {
        userTeamReadRepository = new UserTeamReadRepository(Context);
    }

    /// <summary>
    /// Получает пустой список <see cref="UserTeam"/> по идентификатору организации
    /// </summary>
    [Fact]
    public async Task GetByTeamIdsShouldReturnEmpty()
    {
        //Arrange
        await Context.AddRangeAsync(TestDataGenerator.GetUserTeam(),
            TestDataGenerator.GetUserTeam(),
            TestDataGenerator.GetUserTeam());
        await Context.SaveChangesAsync();

        // Act
        var result = await userTeamReadRepository.GetByTeamIdsAsync(Array.Empty<Guid>(), CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    /// <summary>
    /// Получает список <see cref="UserTeam"/> из одного элемента по идентификатору организации
    /// </summary>
    [Fact]
    public async Task GetByTeamIdsShouldReturnOne()
    {
        //Arrange
        var targetId = Guid.NewGuid();
        var userTeam1 = TestDataGenerator.GetUserTeam(x => x.TeamId = targetId);
        await Context.AddRangeAsync(userTeam1,
            TestDataGenerator.GetUserTeam(x =>
            {
                x.TeamId = targetId;
                x.DeletedAt = DateTimeOffset.UtcNow;
            }),
            TestDataGenerator.GetUserTeam());
        await Context.SaveChangesAsync();

        // Act
        var result = await userTeamReadRepository.GetByTeamIdsAsync(new[] { targetId, }, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeEmpty()
            .And.HaveCount(1)
            .And.ContainSingle(x => x.Id == userTeam1.Id);
    }

    /// <summary>
    /// Получает список <see cref="UserTeam"/> по идентификатору организации
    /// </summary>
    [Fact]
    public async Task GetByTeamIdsShouldReturnValue()
    {
        //Arrange
        var targetId = Guid.NewGuid();
        var userTeam1 = TestDataGenerator.GetUserTeam(x => x.TeamId = targetId);
        var userTeam2 = TestDataGenerator.GetUserTeam();
        await Context.AddRangeAsync(userTeam1,
            userTeam2,
            TestDataGenerator.GetUserTeam(x =>
            {
                x.TeamId = targetId;
                x.DeletedAt = DateTimeOffset.UtcNow;
            }),
            TestDataGenerator.GetUserTeam());
        await Context.SaveChangesAsync();

        // Act
        var result = await userTeamReadRepository.GetByTeamIdsAsync(new[] { targetId, userTeam2.TeamId, }, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeEmpty()
            .And.HaveCount(2)
            .And.ContainSingle(x => x.Id == userTeam1.Id)
            .And.ContainSingle(x => x.Id == userTeam2.Id);
    }
}
