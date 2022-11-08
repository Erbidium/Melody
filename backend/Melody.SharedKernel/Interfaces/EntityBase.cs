namespace Melody.SharedKernel.Interfaces;

public interface IEntityBase<TId>
{
    public TId Id { get; }
}