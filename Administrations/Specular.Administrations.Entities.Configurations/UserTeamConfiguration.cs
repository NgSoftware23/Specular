using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NgSoftware.Specular.Context.EntityFrameworkCore;

namespace NgSoftware.Specular.Administrations.Entities.Configurations;

/// <summary>Конфигурация <see cref="UserTeam"/></summary>
public class UserTeamConfiguration : IEntityTypeConfiguration<UserTeam>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<UserTeam> builder)
    {
        builder.ToTable("UserTeams");
        builder.HasIdAsKey();
        builder.PropertyAuditConfiguration();

        builder.HasIndex(x => new { x.UserId, x.TeamId, }, $"IX_{nameof(UserTeam)}_{nameof(UserTeam.Team)}")
            .IsUnique()
            .HasFilter($@"""{nameof(User.DeletedAt)}"" IS NULL");
    }
}
