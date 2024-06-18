namespace Core.Domain.Common.Interfaces;

public interface IHasDomainEvents
{
    public IReadOnlyList<IDomainEvent> PopDomainEvents();
}