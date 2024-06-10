namespace Core.Domain.Common.Interfaces;

public interface IAuditableEntity
{
    public DateTime CreatedOnUtc { get; }
    public DateTime ModifiedOnUtc { get; }
}