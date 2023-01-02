using LanguageExt.Common;
using Melody.Core.Entities;
using Melody.Core.ValueObjects;

namespace Melody.Core.Interfaces;

public interface ISongService
{
    Task<Result<Song>> Upload(Stream uploadedSoundFile, NewSongData newSongData);
    Task<Result<IReadOnlyCollection<FavouriteSong>>> GetRecommendedSongs(long userId, int page = 1, int pageSize = 10);
}