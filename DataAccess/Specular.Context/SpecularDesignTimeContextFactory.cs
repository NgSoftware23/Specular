using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace NgSoftware.Specular.Context;

/// <summary>
/// Фабрика для создания контекста в DesignTime
/// </summary>
public class SpecularDesignTimeContextFactory : IDesignTimeDbContextFactory<SpecularContext>
{
    /// <summary>
    /// Creates a new instance of a derived context
    /// </summary>
    /// <remarks>
    /// 1) dotnet tool install --global dotnet-ef
    /// 2) dotnet tool update --global dotnet-ef
    /// 3) dotnet ef migrations add [name] --project DataAccess\Specular.Context\Specular.Context.csproj
    /// 4) dotnet ef database update --project DataAccess\Specular.Context\Specular.Context.csproj
    /// 5) dotnet ef database update [targetMigrationName] --project DataAccess\Specular.Context\Specular.Context.csproj
    /// </remarks>
    public SpecularContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<SpecularContext>()
            .UseNpgsql("Host=localhost;Port=5432;Database=specular;Username=postgres;Password=Qwerty123456!")
            .Options;

        return new SpecularContext(options);
    }
}
