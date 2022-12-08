using Melody.Core.Entities;
using Melody.SharedKernel.Interfaces;

namespace Melody.Core.Interfaces;

public interface ISongRepository : IRepository<Song, long>
{
    public Task<long> GetTotalBytesSumUploadsByUser(long userId);
    public Task<IReadOnlyCollection<Song>> GetSongsUploadedByUserId(long userId);
    public Task<IReadOnlyCollection<Song>> GetSongsForPlaylistToAdd(long playlistId, long userId);
    public Task<IReadOnlyCollection<Song>> GetFavouriteUserSongs(long userId);
    public Task CreateFavouriteSong(long id, long userId);
    public Task DeleteFavouriteSong(long id, long userId);
}