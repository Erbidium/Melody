using Melody.Core.Entities;

namespace Melody.Core.Interfaces;

public interface IGenreRepository
{
    Task<IReadOnlyCollection<Genre>> GetAll();
}