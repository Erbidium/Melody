using Melody.Core.Entities;
using Melody.Core.ValueObjects;

namespace Melody.Infrastructure.Data.Repositories;

public interface ISongRepository
{
    Task<Song> CreateSong(SongInfo song);
    Task<Song> GetSong(long id);
    Task<IEnumerable<Song>> GetSongs();
}