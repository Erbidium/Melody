namespace Melody.SharedKernel;

public abstract class EntityBase<TId>
{
    public TId Id { get; set; }
}