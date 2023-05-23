using FluentAssertions;
using NgSoftware.Specular.Administrations.Entities;
using NgSoftware.Specular.Administrations.Repositories.Contracts;
using NgSoftware.Specular.Context.Tests;
using Xunit;

namespace NgSoftware.Specular.Administrations.Repositories.Tests;

/// <summary>
/// Тесты на <see cref="IUserOrganizationReadRepository"/>
/// </summary>
public class UserOrganizationReadRepositoryTests : SpecularContextInMemory
{
    private readonly IUserOrganizationReadRepository userOrganizationReadRepository;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="UserOrganizationReadRepositoryTests"/>
    /// </summary>
    public UserOrganizationReadRepositoryTests()
    {
        userOrganizationReadRepository = new UserOrganizationReadRepository(Context);
    }

    /// <summary>
    /// Получает <see cref="UserOrganization"/> по идентификаторам пользователя и организации
    /// </summary>
    [Fact]
    public async Task GetByUserAndOrganizationIdShouldReturnValue()
    {
        //Arrange
        var targetUserId = Guid.NewGuid();
        var targetOrganizationId = Guid.NewGuid();
        var userOrganization1 = TestDataGenerator.GetUserOrganization(x =>
        {
            x.UserId = targetUserId;
            x.OrganizationId = targetOrganizationId;
        });
        await Context.AddRangeAsync(userOrganization1,
            TestDataGenerator.GetUserOrganization(x =>
            {
                x.UserId = targetUserId;
                x.OrganizationId = targetOrganizationId;
                x.DeletedAt = DateTimeOffset.UtcNow;
            }),
            TestDataGenerator.GetUserOrganization(x => x.UserId = targetUserId),
            TestDataGenerator.GetUserOrganization(x => x.OrganizationId = targetOrganizationId));
        await Context.SaveChangesAsync();

        // Act
        var result = await userOrganizationReadRepository.GetByUserAndOrganizationIdAsync(targetUserId, targetOrganizationId,
            CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.BeEquivalentTo(userOrganization1);
    }

    /// <summary>
    /// Получает список <see cref="UserOrganization"/> по идентификатору организации
    /// </summary>
    [Fact]
    public async Task GetByOrganizationIdShouldReturnValue()
    {
        //Arrange
        var targetOrganizationId = Guid.NewGuid();
        var userOrganization1 = TestDataGenerator.GetUserOrganization(x =>
        {
            x.OrganizationId = targetOrganizationId;
        });
        await Context.AddRangeAsync(userOrganization1,
            TestDataGenerator.GetUserOrganization(x =>
            {
                x.OrganizationId = targetOrganizationId;
                x.DeletedAt = DateTimeOffset.UtcNow;
            }),
            TestDataGenerator.GetUserOrganization());
        await Context.SaveChangesAsync();

        // Act
        var result = await userOrganizationReadRepository.GetByOrganizationIdAsync(targetOrganizationId, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeEmpty()
            .And.HaveCount(1)
            .And.ContainSingle(x => x.Id == userOrganization1.Id);
    }
}
