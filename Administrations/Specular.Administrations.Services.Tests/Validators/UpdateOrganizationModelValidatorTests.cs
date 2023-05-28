using FluentValidation.TestHelper;
using Moq;
using NgSoftware.Specular.Administrations.Repositories.Contracts;
using NgSoftware.Specular.Administrations.Services.Contracts.Models.Organizations;
using NgSoftware.Specular.Administrations.Services.Validators;
using Xunit;

namespace NgSoftware.Specular.Administrations.Services.Tests.Validators;

/// <summary>
/// Тесты для <see cref="UpdateOrganizationModelValidator"/>
/// </summary>
public class UpdateOrganizationModelValidatorTests
{
    private readonly UpdateOrganizationModelValidator validator;
    private readonly Mock<IOrganizationReadRepository> organizationReadRepositoryMock;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="UpdateOrganizationModelValidatorTests"/>
    /// </summary>
    public UpdateOrganizationModelValidatorTests()
    {
        organizationReadRepositoryMock = new Mock<IOrganizationReadRepository>();
        validator = new UpdateOrganizationModelValidator(organizationReadRepositoryMock.Object);
    }

    /// <summary>
    /// Тест на ошибки
    /// </summary>
    [Fact]
    public async Task ShouldHaveErrorMessage()
    {
        //Arrange
        var model = new UpdateOrganizationModel();

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    /// <summary>
    /// Тест на ошибки существования организации
    /// </summary>
    [Fact]
    public async Task ShouldHaveNameExistsErrorMessage()
    {
        //Arrange
        var model = new UpdateOrganizationModel
        {
            Name = $"Name{Guid.NewGuid()}",
            Description = $"Description{Guid.NewGuid()}",
            UserId = Guid.NewGuid(),
            Id = Guid.NewGuid(),
        };
        organizationReadRepositoryMock.Setup(x => x.GetActiveByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestDataGenerator.Organization());

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    /// <summary>
    /// Тест на ошибки существования организации
    /// </summary>
    [Fact]
    public async Task ShouldHaveNoErrors()
    {
        //Arrange
        var model = new UpdateOrganizationModel
        {
            Name = $"Name{Guid.NewGuid()}",
            Description = $"Description{Guid.NewGuid()}",
            UserId = Guid.NewGuid(),
            Id = Guid.NewGuid(),
        };
        organizationReadRepositoryMock.Setup(x => x.GetActiveByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestDataGenerator.Organization(x => x.Id = model.Id));

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }
}
