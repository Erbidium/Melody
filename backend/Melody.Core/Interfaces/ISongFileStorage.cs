namespace Melody.Core.Interfaces;

public interface ISongFileStorage
{
    Task<(string path, TimeSpan duration)> UploadAsync(Stream uploadedSoundFile, string songExtension);
}