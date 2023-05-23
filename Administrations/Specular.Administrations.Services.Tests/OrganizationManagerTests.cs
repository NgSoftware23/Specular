using AutoMapper;
using FluentAssertions;
using NgSoftware.Specular.Administrations.Entities.Enums;
using NgSoftware.Specular.Administrations.Repositories;
using NgSoftware.Specular.Administrations.Services.AutoMappers;
using NgSoftware.Specular.Administrations.Services.Contracts.Exceptions;
using NgSoftware.Specular.Administrations.Services.Contracts.Interfaces;
using NgSoftware.Specular.Administrations.Services.Contracts.Models.Organizations;
using NgSoftware.Specular.Administrations.Services.Organization;
using NgSoftware.Specular.Context.Tests;
using Xunit;

namespace NgSoftware.Specular.Administrations.Services.Tests;

/// <summary>
/// Тесты на <see cref="IOrganizationManager"/>
/// </summary>
public class OrganizationManagerTests : SpecularContextInMemory
{
    private readonly IOrganizationManager organizationManager;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="OrganizationManagerTests"/>
    /// </summary>
    public OrganizationManagerTests()
    {
        var config = new MapperConfiguration(opts =>
        {
            opts.AddProfile(new AdministrationServiceProfile());
        });
        organizationManager = new OrganizationManager(new OrganizationReadRepository(Context),
            new OrganizationWriteRepository(WriterContext),
            new UserOrganizationReadRepository(Context),
            new UserOrganizationWriteRepository(WriterContext),
            new TeamReadRepository(Context),
            new TeamWriteRepository(WriterContext),
            new UserTeamReadRepository(Context),
            new UserTeamWriteRepository(WriterContext),
            Context,
            config.CreateMapper());
    }

    /// <summary>
    /// Добавление организации работает
    /// </summary>
    [Fact]
    public async Task CreateShouldWork()
    {
        //Arrange
        var model = new CreateOrganizationModel
        {
            Name = $"Name{Guid.NewGuid():N}",
            Description = $"Description{Guid.NewGuid():N}",
            UserId = Guid.NewGuid(),
        };

        // Act
        var result = await organizationManager.CreateAsync(model, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.BeEquivalentTo(new { model.Name, model.Description, });
        var userOrganization = Context.Set<Entities.UserOrganization>().FirstOrDefault(x => x.OrganizationId == result.Id);
        userOrganization.Should()
            .NotBeNull()
            .And.BeEquivalentTo(new { model.UserId, Role = Role.Admin, });
    }

    /// <summary>
    /// Получает список организаций для указанного идентификатора пользователя
    /// </summary>
    [Fact]
    public async Task GetByUserIdShouldReturnValue()
    {
        //Arrange
        var targetUserId = Guid.NewGuid();
        var organization1 = TestDataGenerator.Organization();
        var organization2 = TestDataGenerator.Organization();
        organization2.DeletedAt = DateTimeOffset.UtcNow;
        var organization3 = TestDataGenerator.Organization();
        var organization4 = TestDataGenerator.Organization();
        await Context.AddRangeAsync(organization1, organization2, organization3, organization4);
        await Context.AddRangeAsync(TestDataGenerator.UserOrganization(x =>
            {
                x.UserId = targetUserId;
                x.OrganizationId = organization1.Id;
            }),
            TestDataGenerator.UserOrganization(x =>
            {
                x.UserId = targetUserId;
                x.OrganizationId = organization2.Id;
            }),
            TestDataGenerator.UserOrganization(x =>
            {
                x.UserId = targetUserId;
                x.OrganizationId = organization3.Id;
                x.DeletedAt = DateTimeOffset.UtcNow;
            }),
            TestDataGenerator.UserOrganization(x =>
            {
                x.UserId = targetUserId;
                x.OrganizationId = organization4.Id;
            }));

        await Context.SaveChangesAsync();

        // Act
        var result = await organizationManager.GetByUserIdAsync(targetUserId, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeEmpty()
            .And.HaveCount(2)
            .And.ContainSingle(x => x.Id == organization1.Id)
            .And.ContainSingle(x => x.Id == organization4.Id);
    }

    /// <summary>
    /// Редактирование организации выдаёт ошибку: организация не найдена
    /// </summary>
    [Fact]
    public Task UpdateShouldThrowNotFound()
    {
        //Arrange
        var model = GetModel();

        // Act
        Func<Task> act = () => organizationManager.UpdateAsync(model, CancellationToken.None);

        // Assert
        return act.Should().ThrowAsync<AdministrationEntityNotFoundException<Entities.Organization>>()
            .WithMessage($"*{model.Id}*");
    }

    /// <summary>
    /// Редактирование организации выдаёт ошибку: не хватает прав
    /// </summary>
    [Theory]
    [InlineData(Role.Header)]
    [InlineData(Role.Participant)]
    [InlineData(Role.ScrumMaster)]
    public async Task UpdateShouldThrowDeny(Role targetRole)
    {
        //Arrange
        var model = GetModel();
        var organization1 = TestDataGenerator.Organization();
        organization1.Id = model.Id;
        await Context.AddRangeAsync(organization1, TestDataGenerator.UserOrganization(x =>
        {
            x.UserId = model.UserId;
            x.OrganizationId = organization1.Id;
            x.Role = targetRole;
        }));
        await Context.SaveChangesAsync();

        // Act
        Func<Task> act = () => organizationManager.UpdateAsync(model, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdministrationInvalidOperationException>()
            .WithMessage($"*прав*");
    }

    /// <summary>
    /// Редактирование организации работает
    /// </summary>
    [Fact]
    public async Task UpdateShouldWork()
    {
        //Arrange
        var model = GetModel();
        var organization1 = TestDataGenerator.Organization();
        organization1.Id = model.Id;
        await Context.AddRangeAsync(organization1, TestDataGenerator.UserOrganization(x =>
        {
            x.UserId = model.UserId;
            x.OrganizationId = organization1.Id;
            x.Role = Role.Admin;
        }));
        await UnitOfWork.SaveChangesAsync();

        // Act
        var result = await organizationManager.UpdateAsync(model, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And.BeEquivalentTo(new { model.Name, model.Description, });
    }

    /// <summary>
    /// Удаление организации работает
    /// </summary>
    [Fact]
    public async Task DeleteShouldWork()
    {
        //Arrange
        var model = new DeleteOrganizationModel { UserId = Guid.NewGuid(), OrganizationId = Guid.NewGuid(), };
        var organization1 = TestDataGenerator.Organization();
        organization1.Id = model.OrganizationId;
        var targetTeam = TestDataGenerator.Team(x => x.OrganizationId = organization1.Id);
        var targetUser = TestDataGenerator.CreateUser(x => x.Id = model.UserId);
        var targetUserOrganization = TestDataGenerator.UserOrganization(x =>
        {
            x.UserId = model.UserId;
            x.OrganizationId = organization1.Id;
            x.Role = Role.Admin;
        });
        var targetUserTeam = TestDataGenerator.UserTeam(x =>
        {
            x.UserId = targetUser.Id;
            x.TeamId = targetTeam.Id;
        });
        await Context.AddRangeAsync(organization1,
            targetUser,
            targetUserOrganization,
            targetTeam,
            targetUserTeam);
        await UnitOfWork.SaveChangesAsync();

        // Act
        Func<Task> act = () =>  organizationManager.DeleteAsync(model, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
        var entityOrganization = Context.Set<Entities.Organization>().First(x => x.Id == organization1.Id);
        entityOrganization.DeletedAt.Should().NotBeNull();
        var entityUserOrganization = Context.Set<Entities.UserOrganization>().First(x => x.Id == targetUserOrganization.Id);
        entityUserOrganization.DeletedAt.Should().NotBeNull();
        var entityTeam = Context.Set<Entities.Team>().First(x => x.Id == targetTeam.Id);
        entityTeam.DeletedAt.Should().NotBeNull();
        var entityUserTeam = Context.Set<Entities.UserTeam>().First(x => x.Id == targetUserTeam.Id);
        entityUserTeam.DeletedAt.Should().NotBeNull();
        var entityUser = Context.Set<Entities.User>().First(x => x.Id == targetUser.Id);
        entityUser.DeletedAt.Should().BeNull();
    }

    private static UpdateOrganizationModel GetModel(Action<UpdateOrganizationModel>? settings = null)
    {
        var result = new UpdateOrganizationModel
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Name = $"Name{Guid.NewGuid():N}",
            Description = $"Description{Guid.NewGuid():N}",
        };

        settings?.Invoke(result);
        return result;
    }
}
