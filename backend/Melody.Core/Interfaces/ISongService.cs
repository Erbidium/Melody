using LanguageExt.Common;
using Melody.Core.Entities;
using Melody.Core.ValueObjects;

namespace Melody.Core.Interfaces;

public interface ISongService
{
    Task<Result<Song>> Upload(Stream uploadedSoundFile, NewSongData newSongData);
}