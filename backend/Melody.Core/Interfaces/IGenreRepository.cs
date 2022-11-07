using Melody.Core.Entities;

namespace Melody.Core.Interfaces;

public interface IGenreRepository
{
    Task<Genre?> GetById(long id);
    Task<IReadOnlyCollection<Genre>> GetAll();
}
