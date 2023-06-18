using Xunit;

namespace NgSoftware.Specular.Api.Tests.Infrastructures;

/// <summary>
/// Колекция для интеграционных тестов ФинтехАпи
/// </summary>
[CollectionDefinition(nameof(SpecularApiTestCollection))]
public class SpecularApiTestCollection
    : ICollectionFixture<SpecularApiFixture>
{

}
