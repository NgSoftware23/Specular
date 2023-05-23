using FluentAssertions;
using NgSoftware.Specular.Administrations.Repositories.Contracts;
using NgSoftware.Specular.Context.Tests;
using Xunit;

namespace NgSoftware.Specular.Administrations.Repositories.Tests;

/// <summary>
/// Тесты на <see cref="IRefreshTokenReadRepository"/>
/// </summary>
public class RefreshTokenReadRepositoryTests : SpecularContextInMemory
{
    private readonly IRefreshTokenReadRepository repository;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="RefreshTokenReadRepositoryTests"/>
    /// </summary>
    public RefreshTokenReadRepositoryTests()
    {
        repository = new RefreshTokenReadRepository(Context);
    }

    /// <summary>
    /// Возвращает пустой элемент по Id при отсутствии данных
    /// </summary>
    [Fact]
    private async Task GetByIdShouldReturnNull()
    {
        //Arrange
        var id = Guid.NewGuid();

        // Act
        var result = await repository.GetByIdAsync(id, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    /// <summary>
    /// Возвращает элемент при запросе по id
    /// </summary>
    [Fact]
    public async Task GetByIdShouldReturnValue()
    {
        //Arrange
        var targetToken = TestDataGenerator.GetRefreshToken();
        await Context.AddAsync(TestDataGenerator.GetRefreshToken(), CancellationToken.None);
        await Context.AddAsync(targetToken, CancellationToken.None);
        await Context.SaveChangesAsync(CancellationToken.None);

        // Act
        var result = await repository.GetByIdAsync(targetToken.Id, CancellationToken.None);

        // Assert
        result.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(targetToken);
    }

    /// <summary>
        /// Возвращает пустой активный элемент по Id при отсутствии данных
        /// </summary>
        [Fact]
        private async Task GetActualByIdShouldReturnNull()
        {
            //Arrange
            var id = Guid.NewGuid();

            // Act
            var result = await repository.GetActualByIdAsync(id, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        /// <summary>
        /// Возвращает пустой активный элемент по Id при отсутствии данных
        /// </summary>
        [Fact]
        public async Task GetActualByIdRemovedShouldReturnNull()
        {
            //Arrange
            var targetToken = TestDataGenerator.GetRefreshToken();
            targetToken.DeletedAt = DateTimeOffset.MaxValue;
            await Context.AddAsync(targetToken, CancellationToken.None);
            await Context.SaveChangesAsync(CancellationToken.None);

            // Act
            var result = await repository.GetActualByIdAsync(targetToken.Id, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        /// <summary>
        /// Возвращает активный элемент по Id
        /// </summary>
        [Fact]
        public async Task GetActualByIdShouldReturnValue()
        {
            //Arrange
            var targetToken = TestDataGenerator.GetRefreshToken();
            await Context.AddAsync(TestDataGenerator.GetRefreshToken(), CancellationToken.None);
            await Context.AddAsync(targetToken, CancellationToken.None);
            await Context.SaveChangesAsync(CancellationToken.None);

            // Act
            var result = await repository.GetActualByIdAsync(targetToken.Id, CancellationToken.None);

            // Assert
            result.Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(targetToken);
        }

        /// <summary>
        /// Возвращает пустой активный элемент по Id пользователя при отсутствии данных
        /// </summary>
        [Fact]
        private async Task GetActualByUserIdShouldReturnNull()
        {
            //Arrange
            var id = Guid.NewGuid();

            // Act
            var result = await repository.GetActualByUserIdAsync(id, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        /// <summary>
        /// Возвращает пустой активный элемент по Id пользователя при отсутствии данных
        /// </summary>
        [Fact]
        public async Task GetActualByUserIdRemovedShouldReturnNull()
        {
            //Arrange
            var targetToken = TestDataGenerator.GetRefreshToken();
            targetToken.DeletedAt = DateTimeOffset.MaxValue;
            await Context.AddAsync(targetToken, CancellationToken.None);
            await Context.SaveChangesAsync(CancellationToken.None);

            // Act
            var result = await repository.GetActualByUserIdAsync(targetToken.UserId, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        /// <summary>
        /// Возвращает активный элемент по Id пользователя
        /// </summary>
        [Fact]
        public async Task GetActualByUserIdShouldReturnValue()
        {
            //Arrange
            var targetToken = TestDataGenerator.GetRefreshToken();
            await Context.AddAsync(targetToken, CancellationToken.None);
            await Context.SaveChangesAsync(CancellationToken.None);

            // Act
            var result = await repository.GetActualByUserIdAsync(targetToken.UserId, CancellationToken.None);

            // Assert
            result.Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(targetToken);
        }
}
