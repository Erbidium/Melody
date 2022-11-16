namespace Melody.SharedKernel.Interfaces;

public interface IRepository<T, TId> where T : IEntityBase<TId>
{
    Task<T> Create(T song);
    Task<T?> GetById(TId id);
    Task<IReadOnlyCollection<T>> GetAll();
    Task Update(T song);
    Task Delete(TId id);
}