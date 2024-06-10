// ReSharper disable ConvertToPrimaryConstructor

using Core.Domain.Common.Interfaces;

namespace Core.Domain.Common.Models;

public abstract class Entity<TEntityId> : IEquatable<Entity<TEntityId>>, IHasDomainEvents
    where TEntityId : notnull
{
    private readonly List<IDomainEvent> _domainEvents = [];

    protected Entity(TEntityId id) => Id = id;

    protected Entity()
    {
    }

    [JsonProperty("id")]
    public TEntityId Id { get; }

    [JsonIgnore]
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    
    public static bool operator ==(Entity<TEntityId> left, Entity<TEntityId> right) => Equals(left, right);

    public static bool operator !=(Entity<TEntityId> left, Entity<TEntityId> right) => !Equals(left, right);

    public bool Equals(Entity<TEntityId>? other)
    {
        return Equals((object?)other);
    }

    public override bool Equals(object? obj)
    {
        return obj is Entity<TEntityId> entity && Id.Equals(entity.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
    
    internal void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}