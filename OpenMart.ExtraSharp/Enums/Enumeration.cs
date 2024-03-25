namespace OpenMart.ExtraSharp.Enums;

public abstract class Enumeration<TId, T> : IComparable
    where TId : IComparable, IComparable<TId>
    where T : IComparable, IComparable<T>
{
    public TId Id { get; }
    public T Value { get; }

    protected Enumeration(TId id, T value)
    {
        Id = id;
        Value = value;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Enumeration<TId, T> otherValue)
        {
            return false;
        }

        var typeMatches = this.GetType() == obj.GetType();
        var valueMatches = Id.Equals(otherValue.Id);

        return typeMatches && valueMatches;
    }

    public override int GetHashCode() => this.Id.GetHashCode() ^ this.Value.GetHashCode();
    public int CompareTo(object? obj) => (obj == null) ? 0 : this.Id.CompareTo(((Enumeration<TId, T>)obj).Id);
    public override string ToString() => this.Value as string 
                                         ?? base.ToString() 
                                         ?? throw new InvalidOperationException();
    public static implicit operator T(Enumeration<TId, T> value)
    {
        return value.Value;
    }
}