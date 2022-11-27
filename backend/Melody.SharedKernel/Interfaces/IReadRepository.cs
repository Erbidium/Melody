namespace Melody.SharedKernel.Interfaces;

public interface IReadRepository<T, TId> where T : EntityBase<TId>
{
    Task<T?> GetById(TId id);
    Task<IReadOnlyCollection<T>> GetAll();
}