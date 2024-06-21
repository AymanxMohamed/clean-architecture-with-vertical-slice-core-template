using ProjectName.Domain.Aggregates.UserAggregate;
using ProjectName.Domain.Aggregates.UserAggregate.ValueObjects;
using ProjectName.Domain.Common.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProjectName.Infrastructure.Persistence.Users.Configurations;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        ConfigureUsersTable(builder);
    }
    
    private static void ConfigureUsersTable(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(g => g.Id);
        
        builder.Property(g => g.Id)
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value));

        builder.Property(g => g.FirstName).HasMaxLength(100);

        builder.Property(g => g.LastName).HasMaxLength(100);
        
        builder.Property(u => u.Email);
        
        builder.Property("_passwordHash").HasColumnName("PasswordHash");

        builder
            .HasMany<User>()
            .WithOne(x => x.CreatedBy)
            .HasForeignKey(x => x.CreatedById);
        
        builder
            .HasMany<User>()
            .WithOne(x => x.ModifiedBy)
            .HasForeignKey(x => x.ModifiedById);
    }
}