using NgSoftware.Specular.Administrations.Entities;
using NgSoftware.Specular.Administrations.Entities.Enums;
using NgSoftware.Specular.Administrations.Services.Contracts.Models.User;

namespace NgSoftware.Specular.Administrations.Services.Tests;

static internal class TestDataGenerator
{
    static internal Entities.User CreateUser(Action<Entities.User>? settings = null)
    {
        var login = $"Login{Guid.NewGuid():N}";
        var result = new Entities.User()
        {
            Id = Guid.NewGuid(),
            Name = $"Name{Guid.NewGuid():N}",
            CreatedBy = $"CreatedBy{Guid.NewGuid():N}",
            Email = $"Email{Guid.NewGuid():N}",
            Login = login,
            LoginLowerCase = login.ToLower(),
            PasswordHash = "WT0qCdXVES9q74kYj3GbQU0kyYkDFiNQA9JazziXqjM=",
            PasswordSalt = "/f0Ji7HVTA4iydlXzHux7Ex9KKIJGssQ0wc6vU0x23g=",
            SecurityStamp = $"SecurityStamp{Guid.NewGuid():N}",
            UpdatedBy = $"UpdatedBy{Guid.NewGuid():N}",
        };
        settings?.Invoke(result);
        return result;
    }

    static internal CreateUserModel CreateUserModel()
        => new()
        {
            Name = $"Name{Guid.NewGuid():N}",
            Email = $"Email{Guid.NewGuid():N}",
            Login = $"Name{Guid.NewGuid():N}",
            Password = $"Password{Guid.NewGuid():N}",
        };

    static internal LoginModel CreateSimpleLoginModel()
        => CreateSimpleLoginModel(null);

    static internal LoginModel CreateSimpleLoginModel(string? login)
        => new() { Login = login ?? "Login", Password = "Password", };

    static internal RefreshToken RefreshToken(Guid userId)
        => new()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            AccessPayload = $"AccessPayload{Guid.NewGuid():N}",
            Expires = DateTimeOffset.MaxValue,
            SecurityStamp = $"SecurityStamp{Guid.NewGuid():N}",
            CreatedAt = DateTimeOffset.UtcNow,
        };

    static internal Entities.Organization Organization(Action<Entities.Organization>? settings = null)
    {
        var name = $"Name{Guid.NewGuid():N}";
        var result = new Entities.Organization()
        {
            Id = Guid.NewGuid(),
            Name = name,
            NameLowerCase = name.ToLower(),
            Description = $"Description{Guid.NewGuid():N}",
        };
        settings?.Invoke(result);
        return result;
    }

    static internal UserOrganization UserOrganization(Action<UserOrganization>? settings)
    {
        var result = new UserOrganization
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            OrganizationId = Guid.NewGuid(),
            Role = Role.Admin,
            CreatedBy = $"CreatedBy{Guid.NewGuid():N}",
            UpdatedBy = $"UpdatedBy{Guid.NewGuid():N}",
        };
        settings?.Invoke(result);
        return result;
    }

    static internal Team Team(Action<Team>? settings = null)
    {
        var name = $"Name{Guid.NewGuid():N}";
        var result = new Team
        {
            Id = Guid.NewGuid(),
            Name = name,
            NameLowerCase = name.ToLower(),
            Description = $"Description{Guid.NewGuid():N}",
            OrganizationId = Guid.NewGuid(),
            CreatedBy = $"CreatedBy{Guid.NewGuid():N}",
            UpdatedBy = $"UpdatedBy{Guid.NewGuid():N}",
        };
        settings?.Invoke(result);
        return result;
    }

    static internal UserTeam UserTeam(Action<UserTeam>? settings)
    {
        var result = new UserTeam
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Role = Role.Admin,
            TeamId = Guid.NewGuid(),
            CreatedBy = $"CreatedBy{Guid.NewGuid():N}",
            UpdatedBy = $"UpdatedBy{Guid.NewGuid():N}",
        };
        settings?.Invoke(result);
        return result;
    }
}
