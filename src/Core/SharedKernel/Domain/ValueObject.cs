namespace Me.Acheddir.Hexagonal.SharedKernel.Domain;

public abstract class ValueObject<T> where T : ValueObject<T>
{
    public override bool Equals(object? obj)
    {
        return obj is T valueObject
               && EqualsInternal(valueObject);
    }

    protected abstract bool EqualsInternal(T other);

    public override int GetHashCode()
    {
        return GetHashCodeInternal();
    }

    protected abstract int GetHashCodeInternal();
    
    public static bool operator ==(ValueObject<T>? a, ValueObject<T>? b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(ValueObject<T>? a, ValueObject<T>? b)
    {
        return !(a == b);
    }
}
