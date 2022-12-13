﻿using Melody.Core.Interfaces;
using NAudio.Wave;

namespace Melody.Infrastructure;

public class SongFileStorage : ISongFileStorage
{
    private const string FolderName = "Sounds";

    public async Task<(string path, TimeSpan duration)> UploadAsync(Stream uploadedSoundFile, string songExtension)
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var pathToSave = Path.Combine(currentDirectory, FolderName);
        if (!Directory.Exists(pathToSave)) Directory.CreateDirectory(pathToSave);
        var guidFileName = Guid.NewGuid() + songExtension;

        var fullPath = Path.Combine(pathToSave, guidFileName);
        await using var stream = new FileStream(fullPath, FileMode.Create);
        await uploadedSoundFile.CopyToAsync(stream);
        var reader = new MediaFoundationReader(fullPath);

        return (Path.Combine(FolderName, guidFileName), reader.TotalTime);
    }
}