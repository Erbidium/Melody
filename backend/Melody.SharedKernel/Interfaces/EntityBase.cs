namespace Melody.SharedKernel.Interfaces;

public abstract class EntityBase<TId>
{
    public TId Id { get; set; }
}