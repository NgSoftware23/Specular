using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NgSoftware.Specular.Context.EntityFrameworkCore;

namespace NgSoftware.Specular.Administrations.Entities.Configurations;

/// <summary>Конфигурация <see cref="Team"/></summary>
public class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.ToTable("Teams");
        builder.HasIdAsKey();
        builder.PropertyAuditConfiguration();

        builder.HasIndex(x => new { x.NameLowerCase, x.OrganizationId }, $"IX_{nameof(Team)}_{nameof(Team.NameLowerCase)}")
            .IsUnique()
            .HasFilter($@"""{nameof(User.DeletedAt)}"" IS NULL");

        builder.HasMany(x => x.Users)
            .WithOne(x => x.Team)
            .HasForeignKey(x => x.TeamId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
