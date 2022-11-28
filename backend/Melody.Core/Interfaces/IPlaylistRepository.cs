using Melody.Core.Entities;

namespace Melody.Core.Interfaces;

public interface IPlaylistRepository
{
    Task<bool> Create(CreatePlaylist song);
    Task<Playlist?> GetById(long id);
    Task<IReadOnlyCollection<Playlist>> GetAll();
    Task Update(UpdatePlaylist playlist);
    Task Delete(long id);
}