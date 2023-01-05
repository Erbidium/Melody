using LanguageExt.Common;
using Melody.Core.Constants;
using Melody.Core.Entities;
using Melody.Core.Exceptions;
using Melody.Core.Interfaces;
using Melody.Core.ValueObjects;

namespace Melody.Core.Services;

public class SongService : ISongService
{
    private readonly ISongFileStorage _songFileStorage;
    private readonly ISongRepository _songRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRecommender _recommender;

    public SongService(ISongRepository songRepository, ISongFileStorage songFileStorage, IUserRepository userRepository,
        IRecommender recommender)
    {
        _songRepository = songRepository;
        _songFileStorage = songFileStorage;
        _userRepository = userRepository;
        _recommender = recommender;
    }

    public async Task<Result<Song>> Upload(Stream uploadedSoundFile, NewSongData newSongData)
    {
        var userUploadsSize = await _songRepository.GetTotalBytesSumUploadsByUser(newSongData.UserId);
        if (userUploadsSize + uploadedSoundFile.Length > SongConstants.UserUploadsLimit)
            return new Result<Song>(new UploadLimitException());

        var (path, duration) = await _songFileStorage.UploadAsync(uploadedSoundFile, newSongData.Extension);

        var song = new Song(
            newSongData.UserId,
            newSongData.Name,
            path,
            newSongData.AuthorName,
            newSongData.Year,
            newSongData.GenreId,
            uploadedSoundFile.Length,
            DateTime.Now,
            duration);
        return await _songRepository.Create(song);
    }

    public async Task<Result<IReadOnlyCollection<FavouriteSong>>> GetRecommendedSongs(long userId, int page = 1,
        int pageSize = 10)
    {
        var recommendationsPreferences = await _userRepository.GetUserRecommendationsPreferences(userId);
        if (recommendationsPreferences is null)
            return new Result<IReadOnlyCollection<FavouriteSong>>(new KeyNotFoundException());

        var recommendedSongsIds = await _recommender.GetRecommendedSongsIds(recommendationsPreferences, page, pageSize);
        return await recommendedSongsIds.Match<Task<Result<IReadOnlyCollection<FavouriteSong>>>>(
            async recommendations =>
                new Result<IReadOnlyCollection<FavouriteSong>>(
                    await _songRepository.GetSongsByIds(recommendations, userId)),
            error => Task.FromResult(new Result<IReadOnlyCollection<FavouriteSong>>(error)));
    }
}