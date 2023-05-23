using NgSoftware.Specular.Administrations.Entities;
using NgSoftware.Specular.Administrations.Entities.Enums;

namespace NgSoftware.Specular.Administrations.Repositories.Tests;

static internal class TestDataGenerator
{
    /// <summary>
    /// Создаёт <see cref="User"/>
    /// </summary>
    static internal User GetUser(Action<User>? action = null)
    {
        var email = $"Email{Guid.NewGuid():N}";
        var login = $"Login{Guid.NewGuid():N}";
        var user = new User()
        {
            Id = Guid.NewGuid(),
            Name = $"Name{Guid.NewGuid():N}",
            CreatedBy = $"CreatedBy{Guid.NewGuid():N}",
            UpdatedBy = $"UpdatedBy{Guid.NewGuid():N}",
            Email = email,
            EmailLowerCase = email.ToLower(),
            EmailConfirmed = true,
            Login = login,
            LoginLowerCase = login.ToLower(),
            PasswordHash = $"PasswordHash{Guid.NewGuid():N}",
            PasswordSalt = $"PasswordSalt{Guid.NewGuid():N}",
            SecurityStamp = $"SecurityStamp{Guid.NewGuid():N}",
        };
        action?.Invoke(user);

        return user;
    }

    static internal UserOrganization GetUserOrganization(Guid userId, Guid organizationId, Action<UserOrganization>? action = null)
    {
        var userOrganization = new UserOrganization
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            OrganizationId = organizationId,
            Role = Role.Admin,
            CreatedBy = $"CreatedBy{Guid.NewGuid():N}",
            UpdatedBy = $"UpdatedBy{Guid.NewGuid():N}",
        };
        action?.Invoke(userOrganization);

        return userOrganization;
    }

    static internal RefreshToken GetRefreshToken()
        => new()
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            AccessPayload = $"AccessPayload{Guid.NewGuid():N}",
            Expires = DateTimeOffset.MaxValue,
            SecurityStamp = $"SecurityStamp{Guid.NewGuid():N}",
            CreatedAt = DateTimeOffset.UtcNow,
        };

    static internal Organization GetOrganization()
    {
        var name = $"Name{Guid.NewGuid():N}";
        return new()
        {
            Id = Guid.NewGuid(),
            Name = name,
            NameLowerCase = name.ToLower(),
            Description = $"Description{Guid.NewGuid():N}",
            CreatedBy = $"CreatedBy{Guid.NewGuid():N}",
            UpdatedBy = $"UpdatedBy{Guid.NewGuid():N}",
        };
    }

    static internal Team GetTeam()
    {
        var name = $"Name{Guid.NewGuid():N}";
        return new()
        {
            Id = Guid.NewGuid(),
            Name = name,
            NameLowerCase = name.ToLower(),
            Description = $"Description{Guid.NewGuid():N}",
            CreatedBy = $"CreatedBy{Guid.NewGuid():N}",
            UpdatedBy = $"UpdatedBy{Guid.NewGuid():N}",
            OrganizationId = Guid.NewGuid(),
        };
    }

    static internal UserOrganization GetUserOrganization(Action<UserOrganization>? settings = null)
    {
        var result = new UserOrganization
        {
            Id = Guid.NewGuid(),
            OrganizationId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            CreatedBy = $"CreatedBy{Guid.NewGuid():N}",
            UpdatedBy = $"UpdatedBy{Guid.NewGuid():N}",
            Role = Role.Admin,
        };
        settings?.Invoke(result);
        return result;
    }

    static internal UserTeam GetUserTeam(Action<UserTeam>? settings = null)
    {
        var result = new UserTeam
        {
            Id = Guid.NewGuid(),
            Role = Role.Admin,
            CreatedBy = $"CreatedBy{Guid.NewGuid():N}",
            UpdatedBy = $"UpdatedBy{Guid.NewGuid():N}",
            UserId = Guid.NewGuid(),
            TeamId = Guid.NewGuid(),
        };
        settings?.Invoke(result);
        return result;
    }
}
