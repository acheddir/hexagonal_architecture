namespace Me.Acheddir.Hexagonal.SharedKernel.Domain;

public abstract class Entity<T>
{
    protected Entity() { }
    
    protected Entity(T? id)
    {
        Id = id;
    }

    protected T? Id { get; }

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
}
