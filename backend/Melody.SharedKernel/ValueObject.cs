namespace Melody.SharedKernel;

public abstract class ValueObject : IEquatable<ValueObject>
{
    public bool Equals(ValueObject? valueObject)
    {
        if (valueObject == null || GetType() != valueObject.GetType())
            return false;

        return GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
    }

    protected abstract IEnumerable<object> GetEqualityComponents();
}