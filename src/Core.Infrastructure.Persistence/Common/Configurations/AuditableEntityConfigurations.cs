using Core.Domain.Aggregates.UserAggregate.ValueObjects;
using Core.Domain.Common.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Infrastructure.Persistence.Common.Configurations;

public class AuditableEntityConfigurations<TEntityId> : IEntityTypeConfiguration<AuditableEntity<TEntityId>>
    where TEntityId : notnull
{
    public void Configure(EntityTypeBuilder<AuditableEntity<TEntityId>> builder)
    {
        builder
            .Property(e => e.CreatedOnUtc)
            .IsRequired();

        builder
            .Property(e => e.ModifiedOnUtc)
            .IsRequired();

        builder
            .Property(e => e.CreatedById)
            .IsRequired(false)
            .HasConversion(
                userId => userId != null ? userId.Value : Guid.Empty,
                userId => userId != Guid.Empty ? UserId.Create(userId) : null);
       
        builder
            .Property(e => e.ModifiedById)
            .IsRequired(false)
            .HasConversion(
                userId => userId != null ? userId.Value : Guid.Empty,
                userId => userId != Guid.Empty ? UserId.Create(userId) : null);

        builder
            .HasOne(e => e.CreatedBy)
            .WithMany()
            .HasForeignKey(e => e.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(e => e.ModifiedBy)
            .WithMany()
            .HasForeignKey(e => e.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}