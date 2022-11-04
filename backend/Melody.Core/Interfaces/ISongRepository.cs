using Melody.Infrastructure.Data.Entities;

namespace Melody.Infrastructure.Data.Repositories
{
    public interface ISongRepository
    {
        Task<Song> CreateSong(Song song);
        Task<Song> GetSong(long id);
        Task<IEnumerable<Song>> GetSongs();
    }
}