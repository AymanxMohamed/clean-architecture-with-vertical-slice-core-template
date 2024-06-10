using Core.Domain.Common.Interfaces;

namespace Core.Domain.Common.Models;

public class AuditableEntity<TEntityId> : Entity<TEntityId>, IAuditableEntity
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
}