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

    public DateTime CreatedOnUtc { get; private set; }
    
    public DateTime ModifiedOnUtc { get; private set; }

    public UserId? CreatedById { get; private set; }
    
    public UserId? ModifiedById { get; private set; }

    public virtual User? CreatedBy { get; private set; }
    
    public virtual User? ModifiedBy { get; private set; }
}