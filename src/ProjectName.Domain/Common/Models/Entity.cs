// ReSharper disable ConvertToPrimaryConstructor

using ProjectName.Domain.Common.Interfaces;

namespace ProjectName.Domain.Common.Models;

public abstract class Entity<TId> : IEquatable<Entity<TId>>, IHasDomainEvents
    where TId : notnull
{
    private readonly List<IDomainEvent> _domainEvents = [];

    protected Entity(TId id) => Id = id;

    protected Entity()
    {
    }

    public TId Id { get; init; }

    public static bool operator ==(Entity<TId> left, Entity<TId> right) => Equals(left, right);

    public static bool operator !=(Entity<TId> left, Entity<TId> right) => !Equals(left, right);

    public bool Equals(Entity<TId>? other)
    {
        return Equals((object?)other);
    }

    public override bool Equals(object? obj)
    {
        return obj is Entity<TId> entity && Id.Equals(entity.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public IReadOnlyList<IDomainEvent> PopDomainEvents()
    {
        var domainEvents = _domainEvents.ToList();

        _domainEvents.Clear();
        
        return domainEvents;
    }

    internal void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}