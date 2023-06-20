using BnA.IAM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BnA.IAM.Infrastructure.Data.Configs;

public sealed class ApplicationUserRoleConfiguration : IEntityTypeConfiguration<ApplicationUserRole>
{
    /// <summary>
    /// TODO: Remove if Identity framework is installed and resolved this entity
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
    {
        builder
            .ToTable("AspNetUserRoles");

        builder.HasKey(e => e.Id);

        // To resolve DB existing nvarchar ID column to Guid
        builder
            .Property(e => e.UserId)
            .HasConversion<string>();

        builder
           .Property(e => e.RoleId)
           .HasConversion<string>();

        builder
            .HasOne(e => e.ApplicationRole)
            .WithMany()
            .HasForeignKey(e => e.RoleId);
    }
}