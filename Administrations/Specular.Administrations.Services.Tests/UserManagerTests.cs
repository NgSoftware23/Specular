using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NgSoftware.Specular.Administrations.Repositories;
using NgSoftware.Specular.Administrations.Services.AutoMappers;
using NgSoftware.Specular.Administrations.Services.Contracts.Exceptions;
using NgSoftware.Specular.Administrations.Services.Contracts.Interfaces;
using NgSoftware.Specular.Administrations.Services.Contracts.Models.User;
using NgSoftware.Specular.Administrations.Services.Resources;
using NgSoftware.Specular.Administrations.Services.User;
using NgSoftware.Specular.Context.Tests;
using Specular.Common.Core.Implementations;
using Xunit;

namespace NgSoftware.Specular.Administrations.Services.Tests;

/// <summary>
/// Тесты для <see cref="IUserManager"/>
/// </summary>
public class UserManagerTests : SpecularContextInMemory
{
    private readonly IUserManager userManager;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="UserManagerTests"/>
    /// </summary>
    public UserManagerTests()
    {
        var config = new MapperConfiguration(opts =>
        {
            opts.AddProfile(new AdministrationServiceProfile());
        });
        var mapper = config.CreateMapper();
        userManager = new UserManager(new UserReadRepository(Context),
            new UserWriteRepository(WriterContext),
            new DateTimeProvider(),
            Context,
            mapper);
    }

    /// <summary>
    /// Ошибка создания нового пользователя, логин уже существует
    /// </summary>
    [Fact]
    public async Task CreateUserShouldThrow()
    {
        //Arrange
        var user = TestDataGenerator.CreateUser();
        await Context.AddAsync(user);
        await Context.SaveChangesAsync(CancellationToken.None);
        var model = TestDataGenerator.CreateUserModel();
        model.Login = user.Login;

        // Act
        Func<Task> act = () => userManager.CreateUserAsync(model, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdministrationInvalidOperationException>();
    }

    /// <summary>
    /// Создаёт нового пользователя без ошибок
    /// </summary>
    [Fact]
    public async Task CreateUserShouldWork()
    {
        //Arrange
        var model = TestDataGenerator.CreateUserModel();

        // Act
        Func<Task> act = () => userManager.CreateUserAsync(model, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
        var user = await Context.Set<Entities.User>().FirstOrDefaultAsync();
        user.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(new
            {
                model.Name,
                model.Email,
                model.Login,
                LoginLowerCase = model.Login.ToLower(),
            });
    }

    /// <summary>
    /// Логин с ошибкой: пользователь не найден
    /// </summary>
    [Fact]
    public Task LoginShouldThrowByNotFound()
    {
        // Act
        Func<Task> act = () => userManager.GetActiveByLoginAndPasswordAsync(TestDataGenerator.CreateSimpleLoginModel(),
            CancellationToken.None);

        // Assert
        return act.Should().ThrowAsync<AdministrationNotFoundException>();
    }

    /// <summary>
    /// Логин с ошибкой: пользователь заблокирован без даты
    /// </summary>
    [Fact]
    public async Task LoginShouldThrowByBlocked()
    {
        //Arrange
        var user = TestDataGenerator.CreateUser();
        user.Blocked = true;
        user.BlockedAt = null;
        await Context.AddAsync(user, CancellationToken.None);
        await Context.SaveChangesAsync(CancellationToken.None);

        // Act
        Func<Task> act = () => userManager.GetActiveByLoginAndPasswordAsync(TestDataGenerator.CreateSimpleLoginModel(),
            CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdministrationNotFoundException>();
    }

    /// <summary>
    /// Логин с ошибкой: пользователь заблокирован с указанием даты
    /// </summary>
    [Fact]
    public async Task LoginShouldThrowByBlockedWithDate()
    {
        //Arrange
        var user = TestDataGenerator.CreateUser();
        user.Blocked = true;
        user.BlockedAt = DateTimeOffset.UtcNow.AddDays(1);
        await Context.AddAsync(user, CancellationToken.None);
        await Context.SaveChangesAsync(CancellationToken.None);

        // Act
        Func<Task> act = () => userManager.GetActiveByLoginAndPasswordAsync(TestDataGenerator.CreateSimpleLoginModel(),
            CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdministrationNotFoundException>();
    }

    /// <summary>
    /// Логин удался
    /// </summary>
    [Fact]
    public async Task LoginShouldWork()
    {
        //Arrange
        var user = TestDataGenerator.CreateUser();
        await Context.AddAsync(user, CancellationToken.None);
        await Context.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await userManager.GetActiveByLoginAndPasswordAsync(TestDataGenerator.CreateSimpleLoginModel(user.Login),
            CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(new
            {
                user.Id,
                user.Name,
                user.SecurityStamp,
                user.Login
            });
    }

    /// <summary>
    /// Логин удался если блокировка кончилась
    /// </summary>
    [Fact]
    public async Task LoginShouldBlockedWork()
    {
        //Arrange
        var user = TestDataGenerator.CreateUser();
        user.Blocked = true;
        user.BlockedAt = DateTimeOffset.UtcNow.AddDays(-1);
        await Context.AddAsync(user, CancellationToken.None);
        await UnitOfWork.SaveChangesAsync();

        // Act
        var result = await userManager.GetActiveByLoginAndPasswordAsync(TestDataGenerator.CreateSimpleLoginModel(user.Login),
            CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(new
            {
                user.Id,
                user.Name,
                user.SecurityStamp,
                user.Login
            });
        var entity = await Context.Set<Entities.User>().SingleOrDefaultAsync(x => x.Id == user.Id);
        entity.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(new
            {
                Blocked = false,
                LoginAttemptFailedCount = 0,
                BlockedAt = (DateTimeOffset?)null,
                Note = string.Empty,
            });
    }

    /// <summary>
    /// Логин удался если блокировка кончилась после неудачных
    /// попыток входа спустя 5 минут
    /// </summary>
    [Fact]
    public async Task LoginShouldBlockedAttemptFailedWork()
    {
        //Arrange
        var user = TestDataGenerator.CreateUser();
        user.Blocked = true;
        user.BlockedAt = DateTimeOffset.UtcNow.AddMinutes(-AdministrationConstants.LoginAttemptMinutesDelay - 1);
        user.LoginAttemptFailedCount = AdministrationConstants.MaximumFailedCount;
        user.Note = AdministrationConstants.FailedCountMessage;
        await Context.AddAsync(user, CancellationToken.None);
        await UnitOfWork.SaveChangesAsync();

        // Act
        var result = await userManager.GetActiveByLoginAndPasswordAsync(TestDataGenerator.CreateSimpleLoginModel(user.Login),
            CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(new
            {
                user.Id,
                user.Name,
                user.SecurityStamp,
                user.Login
            });
        var entity = await Context.Set<Entities.User>().SingleOrDefaultAsync(x => x.Id == user.Id);
        entity.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(new
            {
                Blocked = false,
                LoginAttemptFailedCount = 0,
                BlockedAt = (DateTimeOffset?)null,
                Note = string.Empty,
            });
    }

    /// <summary>
    /// Логин не удался, зафиксированна попытка
    /// </summary>
    [Fact]
    public async Task LoginShouldThrowPasswordFailed()
    {
        //Arrange
        var user = TestDataGenerator.CreateUser();
        await Context.AddAsync(user, CancellationToken.None);
        await UnitOfWork.SaveChangesAsync();
        var model = new LoginModel
        {
            Login = user.Login,
            Password = "failedPassword",
        };

        // Act
        Func<Task> act = () => userManager.GetActiveByLoginAndPasswordAsync(model, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdministrationNotFoundException>();
        var entity = await Context.Set<Entities.User>().SingleOrDefaultAsync(x => x.Id == user.Id);
        entity.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(new
            {
                LoginAttemptFailedCount = 1,
            });
    }

    /// <summary>
    /// Логин не удался, зафиксированна 3я попытка и блокирование
    /// </summary>
    [Fact]
    public async Task LoginShouldThrowBlock()
    {
        //Arrange
        var user = TestDataGenerator.CreateUser();
        await Context.AddAsync(user, CancellationToken.None);
        await UnitOfWork.SaveChangesAsync();
        var model = new LoginModel
        {
            Login = user.Login,
            Password = "failedPassword",
        };

        // Act
        Func<Task> act = () => userManager.GetActiveByLoginAndPasswordAsync(model, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AdministrationNotFoundException>();
        await act.Should().ThrowAsync<AdministrationNotFoundException>();
        await act.Should().ThrowAsync<AdministrationNotFoundException>();
        var entity = await Context.Set<Entities.User>().SingleOrDefaultAsync(x => x.Id == user.Id);
        entity.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(new
            {
                Blocked = true,
                LoginAttemptFailedCount = AdministrationConstants.MaximumFailedCount,
                Note = AdministrationConstants.FailedCountMessage,
            });
    }
}
