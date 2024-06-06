// ReSharper disable ConvertToPrimaryConstructor

using Core.Domain.Common.Interfaces;

namespace Core.Domain.Models;

public abstract class Entity<TId> : IEquatable<Entity<TId>>, IHasDomainEvents
    where TId : notnull
{
    private readonly List<IDomainEvent> _domainEvents = [];

    protected Entity(TId id) => Id = id;

    protected Entity()
    {
    }

    [JsonProperty("id")]
    public TId Id { get; }

    [JsonIgnore]
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    
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

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
    
    internal void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}