using BnA.IAM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BnA.IAM.Infrastructure.Data.Configs;

public sealed class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    /// <summary>
    /// TODO: Remove if Identity framework is installed and resolved this entity
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder
            .ToTable("AspNetRoles");

        // To resolve DB existing nvarchar ID column to Guid
        builder
            .Property(e => e.Id)
            .HasConversion<string>();
    }
}
