namespace Melody.Core.Interfaces;

public interface ISongFileStorage
{
    Task<string> UploadAsync(Stream uploadedSoundFile, string songExtension);
}