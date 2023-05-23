using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NgSoftware.Specular.Administrations.Entities;
using NgSoftware.Specular.Administrations.Repositories;
using NgSoftware.Specular.Administrations.Services.Contracts.Exceptions;
using NgSoftware.Specular.Administrations.Services.Contracts.Interfaces;
using NgSoftware.Specular.Administrations.Services.Contracts.Models.Token;
using NgSoftware.Specular.Administrations.Services.User;
using NgSoftware.Specular.Context.Tests;
using Specular.Common.Core.Implementations;
using Xunit;

namespace NgSoftware.Specular.Administrations.Services.Tests;

/// <summary>
/// Тесты для <see cref="IRefreshTokenManager"/>
/// </summary>
public class RefreshTokenManagerTests : SpecularContextInMemory
{
    private readonly IRefreshTokenManager refreshTokenManager;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="RefreshTokenManagerTests"/>
    /// </summary>
    public RefreshTokenManagerTests()
    {
        refreshTokenManager = new RefreshTokenManager(new RefreshTokenReadRepository(Context),
            new RefreshTokenWriteRepository(WriterContext),
            Context,
            new DateTimeProvider(),
            new UserReadRepository(Context));
    }

    /// <summary>
    /// Создаёт токен обновления
    /// </summary>
    [Fact]
    public async Task CreateRefreshTokenAsyncShouldWork()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var targetToken = TestDataGenerator.RefreshToken(userId);
        await Context.AddAsync(targetToken, CancellationToken.None);
        await UnitOfWork.SaveChangesAsync();
        var model = new CreateRefreshTokenModel
        {
            UserId = userId,
            AccessPayload = $"AccessPayload{Guid.NewGuid():N}",
            ExpiredDays = 10,
            SecurityStamp = $"SecurityStamp{Guid.NewGuid():N}",
        };

        // Act
        var result = await refreshTokenManager.CreateRefreshTokenAsync(model, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();
        var oldToken = await Context.Set<RefreshToken>().FirstOrDefaultAsync(x => x.Id == targetToken.Id, CancellationToken.None);
        oldToken?.DeletedAt.Should().NotBeNull();
        var newToken = await Context.Set<RefreshToken>().FirstOrDefaultAsync(x => x.Id == result, CancellationToken.None);
        newToken.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(new
            {
                UserId = userId,
                model.AccessPayload,
                model.SecurityStamp,
                DeletedAt = (DateTimeOffset?)null,
            });
    }

    /// <summary>
    /// Обновление токена падает с ошибкой: токен не найден
    /// </summary>
    [Fact]
    public async Task UpdateRefreshTokenShouldThrowNotFound()
    {
        //Arrange
        var targetToken = TestDataGenerator.RefreshToken(Guid.NewGuid());
        targetToken.DeletedAt = DateTimeOffset.UtcNow;
        await Context.AddAsync(targetToken, CancellationToken.None);
        await Context.SaveChangesAsync(CancellationToken.None);

        // Act
        Func<Task> act = () => refreshTokenManager.UpdateRefreshTokenAsync(targetToken.Id, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<AdministrationEntityNotFoundException<RefreshToken>>()
            .Where(x => x.Message.Contains(targetToken.Id.ToString()));
    }

    /// <summary>
    /// Обновление токена падает с ошибкой: срок действия токена окончен
    /// </summary>
    [Fact]
    public async Task UpdateRefreshTokenShouldThrowExpires()
    {
        //Arrange
        var targetToken = TestDataGenerator.RefreshToken(Guid.NewGuid());
        targetToken.Expires = DateTimeOffset.UtcNow.AddDays(-1);
        await Context.AddAsync(targetToken, CancellationToken.None);
        await UnitOfWork.SaveChangesAsync();

        // Act
        Func<Task> act = () => refreshTokenManager.UpdateRefreshTokenAsync(targetToken.Id, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<AdministrationInvalidOperationException>()
            .Where(x => x.Message.Contains("окончен", StringComparison.InvariantCultureIgnoreCase));
    }

    /// <summary>
    /// Обновление токена падает с ошибкой: невалидный SecurityStamp
    /// </summary>
    [Fact]
    public async Task UpdateRefreshTokenShouldThrowSecurityStamp()
    {
        //Arrange
        var user = TestDataGenerator.CreateUser();
        await Context.AddAsync(user, CancellationToken.None);
        var targetToken = TestDataGenerator.RefreshToken(user.Id);
        await Context.AddAsync(targetToken, CancellationToken.None);
        await UnitOfWork.SaveChangesAsync();

        // Act
        Func<Task> act = () => refreshTokenManager.UpdateRefreshTokenAsync(targetToken.Id, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<AdministrationInvalidOperationException>()
            .Where(x => x.Message.Contains("Невалидный", StringComparison.InvariantCultureIgnoreCase));
    }

    /// <summary>
    /// Обновление токена падает с ошибкой: пароль временный
    /// </summary>
    [Fact]
    public async Task UpdateRefreshTokenShouldThrowPasswordIsTemporary()
    {
        //Arrange
        var user = TestDataGenerator.CreateUser();
        user.PasswordIsTemporary = true;
        await Context.AddAsync(user, CancellationToken.None);
        var targetToken = TestDataGenerator.RefreshToken(user.Id);
        await Context.AddAsync(targetToken, CancellationToken.None);
        await UnitOfWork.SaveChangesAsync();

        // Act
        Func<Task> act = () => refreshTokenManager.UpdateRefreshTokenAsync(targetToken.Id, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<AdministrationInvalidOperationException>()
            .Where(x => x.Message.Contains("Невалидный", StringComparison.InvariantCultureIgnoreCase));
    }

    /// <summary>
    /// Обновление токена работает
    /// </summary>
    [Fact]
    public async Task UpdateRefreshTokenShouldWork()
    {
        //Arrange
        var user = TestDataGenerator.CreateUser();
        await Context.AddAsync(user, CancellationToken.None);
        var targetToken = TestDataGenerator.RefreshToken(user.Id);
        targetToken.SecurityStamp = user.SecurityStamp;
        targetToken.Expires = DateTimeOffset.UtcNow.AddDays(5);
        await Context.AddAsync(targetToken, CancellationToken.None);
        await UnitOfWork.SaveChangesAsync();

        // Act
        var result = await refreshTokenManager.UpdateRefreshTokenAsync(targetToken.Id, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(new
            {
                ClaimPayload = targetToken.AccessPayload,
            });
        var oldToken = await Context.Set<RefreshToken>().FirstOrDefaultAsync(x => x.Id == targetToken.Id, CancellationToken.None);
        oldToken?.DeletedAt.Should().NotBeNull();
        var newToken = await Context.Set<RefreshToken>().FirstOrDefaultAsync(x => x.Id == result.TokenId, CancellationToken.None);
        newToken.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(new
            {
                UserId = user.Id,
                targetToken.AccessPayload,
                targetToken.SecurityStamp,
                DeletedAt = (DateTimeOffset?)null,
            });
    }

    /// <summary>
    /// Удаление токена не падает, даже если токен не найден
    /// </summary>
    [Fact]
    public Task DeleteRefreshTokenByUserIdShouldNotThrow()
    {
        //Arrange
        var id = Guid.NewGuid();

        // Act
        Func<Task> act = () => refreshTokenManager.DeleteRefreshTokenByUserIdAsync(id, CancellationToken.None);

        // Assert
        return act.Should().NotThrowAsync();
    }

    /// <summary>
    /// Удаление токена работает
    /// </summary>
    [Fact]
    public async Task DeleteRefreshTokenByUserIdShouldWork()
    {
        //Arrange
        var targetToken = TestDataGenerator.RefreshToken(Guid.NewGuid());
        await Context.AddAsync(targetToken, CancellationToken.None);
        await UnitOfWork.SaveChangesAsync();

        // Act
        Func<Task> act = () => refreshTokenManager.DeleteRefreshTokenByUserIdAsync(targetToken.UserId, CancellationToken.None);

        // Assert
        await act.Should().NotThrowAsync();
        var oldToken = await Context.Set<RefreshToken>().FirstOrDefaultAsync(x => x.Id == targetToken.Id, CancellationToken.None);
        oldToken?.DeletedAt.Should().NotBeNull();
    }
}
