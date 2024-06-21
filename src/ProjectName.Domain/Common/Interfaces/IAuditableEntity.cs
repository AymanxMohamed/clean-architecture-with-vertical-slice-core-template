using ProjectName.Domain.Aggregates.UserAggregate.ValueObjects;

namespace ProjectName.Domain.Common.Interfaces;

public interface IAuditableEntity
{
    public DateTime CreatedOnUtc { get; }

    public DateTime ModifiedOnUtc { get; }

    public UserId? CreatedById { get; }
    
    public UserId? ModifiedById { get;  }
}