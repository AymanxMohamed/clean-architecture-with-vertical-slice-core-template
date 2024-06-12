using Core.Domain.Aggregates.UserAggregate.ValueObjects;

namespace Core.Domain.Common.Interfaces;

public interface IAuditableEntity
{
    public DateTime CreatedOnUtc { get; }

    public DateTime ModifiedOnUtc { get; }

    public UserId? CreatedById { get; }
    
    public UserId? ModifiedById { get;  }
}