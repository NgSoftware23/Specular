using FluentAssertions;
using NgSoftware.Specular.Administrations.Repositories.Contracts;
using NgSoftware.Specular.Context.Tests;
using Xunit;

namespace NgSoftware.Specular.Administrations.Repositories.Tests;

/// <summary>
/// Тесты на <see cref="ITeamReadRepository"/>
/// </summary>
public class TeamReadRepositoryTests : SpecularContextInMemory
{
    private readonly ITeamReadRepository teamReadRepository;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="TeamReadRepositoryTests"/>
    /// </summary>
    public TeamReadRepositoryTests()
    {
        teamReadRepository = new TeamReadRepository(Context);
    }

    /// <summary>
    /// Возвращает данные по идентификатору организации
    /// </summary>
    [Fact]
    public async Task GetByOrganizationIdShouldReturnValue()
    {
        //Arrange
        var organizationId = Guid.NewGuid();
        var team1 = TestDataGenerator.GetTeam();
        team1.OrganizationId = organizationId;
        var team2 = TestDataGenerator.GetTeam();
        var team3 = TestDataGenerator.GetTeam();
        team3.OrganizationId = organizationId;
        team3.DeletedAt = DateTimeOffset.UtcNow;
        await Context.AddRangeAsync(team1, team2);
        await Context.SaveChangesAsync();

        // Act
        var result = await teamReadRepository.GetByOrganizationIdAsync(organizationId, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeEmpty()
            .And.HaveCount(1)
            .And.ContainSingle(x => x.Id == team1.Id);
    }
}
