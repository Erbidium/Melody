using LanguageExt;
using Melody.Core.Entities;
using System.Reflection.Metadata.Ecma335;

namespace Melody.Core.Interfaces;

public interface ISongRepository
{
    public Task<Song> Create(Song song);
    public Task<Song?> GetById(long id);
    public Task<IReadOnlyCollection<Song>> GetAll(string searchText, int page = 1, int pageSize = 10);
    public Task Update(Song song);
    public Task<bool> Delete(long id);
    public Task<long> GetTotalBytesSumUploadsByUser(long userId);
    public Task<IReadOnlyCollection<Song>> GetSongsUploadedByUserId(long userId, int page = 1, int pageSize = 10);
    public Task<IReadOnlyCollection<Song>> GetSongsForPlaylistToAdd(long playlistId, long userId);
    public Task<IReadOnlyCollection<Song>> GetFavouriteUserSongs(long userId);
    public Task<IReadOnlyCollection<Song>> GetFavouriteAndUploadedUserSongs(long userId);
    public Task CreateFavouriteSong(long id, long userId);
    public Task<bool> DeleteFavouriteSong(long id, long userId);
    public Task SaveNewSongListening(long id, long userId);
    public Task<bool> DeleteUploadedSong(long id, long userId);
    public Task<IReadOnlyCollection<FavouriteSong>> GetSongsByIds(IReadOnlyCollection<long> ids, long userId);
}