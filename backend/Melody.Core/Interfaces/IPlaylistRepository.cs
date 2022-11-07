using Melody.Core.Entities;

namespace Melody.Core.Interfaces;

public interface IPlaylistRepository
{
    Task<Playlist> Create(Playlist playlist);
    Task<Playlist?> GetById(long id);
    Task<IReadOnlyCollection<Playlist>> GetAll();
    Task Update(Playlist playlist);
    Task Delete(long id);
}
