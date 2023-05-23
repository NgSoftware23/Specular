using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NgSoftware.Specular.Context.EntityFrameworkCore;

namespace NgSoftware.Specular.Administrations.Entities.Configurations;

/// <summary>Конфигурация <see cref="UserOrganization"/></summary>
public class UserOrganizationConfiguration : IEntityTypeConfiguration<UserOrganization>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<UserOrganization> builder)
    {
        builder.ToTable("UserOrganizations");
        builder.HasIdAsKey();
        builder.PropertyAuditConfiguration();

        builder.HasIndex(x => new { x.UserId, x.OrganizationId, }, $"IX_{nameof(UserOrganization)}_{nameof(UserOrganization.User)}")
            .IsUnique()
            .HasFilter($@"""{nameof(User.DeletedAt)}"" IS NULL");
    }
}
