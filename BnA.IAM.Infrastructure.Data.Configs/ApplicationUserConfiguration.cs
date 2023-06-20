using BnA.IAM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BnA.IAM.Infrastructure.Data.Configs;

public sealed class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    /// <summary>
    /// TODO: Remove if Identity framework is installed and resolved this entity
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder
            .ToTable("AspNetUsers");

        // To resolve DB existing nvarchar ID column to Guid
        builder
            .Property(e => e.Id)
            .HasConversion<string>();

        builder
            .HasOne(e => e.PreferredCountry)
            .WithMany();

        builder
            .HasMany(e => e.Roles)
            .WithOne()
            .HasForeignKey(e => e.UserId)
             .Metadata.PrincipalToDependent.SetPropertyAccessMode(PropertyAccessMode.Field);

        builder
           .Property(e => e.Email)
           .HasMaxLength(256)
           .HasColumnName(nameof(ApplicationUser.Email));

        builder
           .Property(e => e.EmailConfirmed)
           .HasColumnName(nameof(ApplicationUser.EmailConfirmed));

        builder
            .Property(e => e.FirstName)
            .HasColumnName(nameof(ApplicationUser.FirstName));

        builder
           .Property(e => e.LastName)
           .HasColumnName(nameof(ApplicationUser.LastName));

        builder
            .Property(e => e.CreatedDateTime)
            .HasColumnName(nameof(ApplicationUser.CreatedDateTime));

        builder
          .Property(e => e.PhoneNumber)
          .HasColumnName(nameof(ApplicationUser.PhoneNumber));

        builder
           .Property(e => e.ActivationStatus)
           .HasColumnName("ActivationStatus");
        builder
           .Property(e => e.BusinessName)
           .HasColumnName("BusinessName");
        builder
           .Property(e => e.StripeCustomerRef)
           .HasColumnName("StripeCustomerRef");
        builder
       .Property(e => e.CustomInvitationNote)
       .HasColumnName(nameof(ApplicationUser.CustomInvitationNote));

        builder
         .Property(e => e.SecurityStamp)
         .HasColumnName("SecurityStamp");
        builder
        .Property(e => e.StripePaymentAttempts)
        .HasColumnName("StripePaymentAttempts");
        builder
            .Property(e => e.SettingId)
            .HasColumnName(nameof(ApplicationUser.SettingId));
    }
}
