using Melody.Core.Entities;

namespace Melody.Infrastructure.Data.Repositories;

public interface ISongRepository
{
    Task<Song> Create(Song song);
    Task<Song> GetById(long id);
    Task<IReadOnlyCollection<Song>> GetAll();
    Task Update(Song song);
    Task Delete(long id);
}