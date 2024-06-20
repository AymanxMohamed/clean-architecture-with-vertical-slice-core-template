using Core.Domain.Aggregates.UserAggregate;
using Core.Domain.Aggregates.UserAggregate.ValueObjects;
using Core.Domain.Common.Interfaces;

namespace Core.Domain.Common.Models;

public abstract class AuditableEntity<TEntityId> : Entity<TEntityId>, IAuditableEntity
    where TEntityId : notnull
{
    protected AuditableEntity(TEntityId id)
        : base(id)
    {
    }

    protected AuditableEntity()
    {
    }

    public DateTime CreatedOnUtc { get; init; }
    
    public DateTime ModifiedOnUtc { get; init; }

    public UserId? CreatedById { get; init; }
    
    public UserId? ModifiedById { get; init; }

    public virtual User? CreatedBy { get; init; }
    
    public virtual User? ModifiedBy { get; init; }
}