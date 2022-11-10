using Melody.Core.Exceptions;
using Melody.SharedKernel.Interfaces;

namespace Melody.Core.Entities;

public class Song : IEntityBase<long>
{
    public Song(long userId, string name, string generatedName, string path, string authorName, int year, long genreId, long sizeBytes, DateOnly uploadedAt, long id = -1)
    {
        Id = id;
        UserId = userId;
        Name = name;
        GeneratedName = generatedName;
        Path = path;
        AuthorName = authorName;
        Year = year;
        GenreId = genreId;
        SizeBytes = sizeBytes;
        UploadedAt = uploadedAt;
    }

    private long _id;
    public long Id
    {
        get => _id < 0 ? throw new WrongIdException() : _id;
        private set => _id = value;
    }
    public long UserId { get; }   
    public string Name { get; }
    public string GeneratedName { get; }
    public string Path { get; }
    public string AuthorName { get; }
    public int Year { get; }
    public long SizeBytes { get; }
    public DateOnly UploadedAt { get; }
    public long GenreId { get; }
}
