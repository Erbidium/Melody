using Melody.Core.Interfaces;
using NAudio.Wave;

namespace Melody.Infrastructure;

public class SongFileStorage : ISongFileStorage
{
    private const string FolderName = "Sounds";
    public async Task<(string path, TimeSpan duration)> UploadAsync(Stream uploadedSoundFile, string songExtension)
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var pathToSave = Path.Combine(currentDirectory, FolderName);
        if (!Directory.Exists(pathToSave))
        {
            Directory.CreateDirectory(pathToSave);
        }
        var guidFileName = Guid.NewGuid() + songExtension;
        var guidSubFolders = string.Empty;
        for (var i = 0; i < 6; i += 2)
        {
            var guidSubstr = guidFileName.Substring(i, 2);
            guidSubFolders = Path.Combine(guidSubFolders, guidSubstr);
            var currentPath = Path.Combine(pathToSave, guidSubFolders);
            if (!Directory.Exists(currentPath))
            {
                Directory.CreateDirectory(currentPath);
            }
        }

        var fullPath = Path.Combine(pathToSave, guidSubFolders, guidFileName);
        await using var stream = new FileStream(fullPath, FileMode.Create);
        await uploadedSoundFile.CopyToAsync(stream);
        var reader = new MediaFoundationReader(fullPath);

        return (Path.Combine(FolderName, guidSubFolders, guidFileName), reader.TotalTime);
    }
}