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

    public SongService(ISongRepository songRepository, ISongFileStorage songFileStorage)
    {
        _songRepository = songRepository;
        _songFileStorage = songFileStorage;
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
}