namespace Me.Acheddir.Hexagonal.SharedKernel.Domain;

public abstract class Entity<T>
{
    private readonly List<DomainEvent> _domainEvents = new();

    protected Entity() { }

    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected Entity(T? id)
    {
        Id = id;
    }

    public T? Id { get; }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity<T> other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (Id != null && other.Id != null && (Id.Equals(default) || other.Id.Equals(default)))
            return false;

        return Id != null && Id.Equals(other.Id);
    }

    public static bool operator ==(Entity<T>? left, Entity<T>? right)
    {
        if (left is null && right is null)
            return true;

        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }

    public static bool operator !=(Entity<T> left, Entity<T>? right)
    {
        return !(left == right);
    }

    public override int GetHashCode()
    {
        return Id is not null ? Id.GetHashCode() : default;
    }
    
    public void AddDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
