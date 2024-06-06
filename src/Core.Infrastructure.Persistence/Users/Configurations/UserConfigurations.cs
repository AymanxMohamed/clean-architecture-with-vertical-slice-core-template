using Core.Domain.Aggregates.UserAggregate;
using Core.Domain.Aggregates.UserAggregate.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Infrastructure.Persistence.Users.Configurations;

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
    }
}