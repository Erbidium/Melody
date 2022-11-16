namespace Melody.SharedKernel.Interfaces;

public interface IReadRepository<T, TId> where T : IEntityBase<TId>
{
    Task<T?> GetById(TId id);
    Task<IReadOnlyCollection<T>> GetAll();
}