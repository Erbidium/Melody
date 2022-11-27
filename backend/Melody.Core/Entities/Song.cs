using Melody.SharedKernel.Interfaces;

namespace Melody.Core.Entities;

public class Song : EntityBase<long>
{
    public Song(long userId, string name, string path, string authorName, int year, long genreId, long sizeBytes,
        DateTime uploadedAt, TimeSpan duration)
    {
        UserId = userId;
        Name = name;
        Path = path;
        AuthorName = authorName;
        Year = year;
        GenreId = genreId;
        SizeBytes = sizeBytes;
        UploadedAt = uploadedAt;
        Duration = duration;
    }
    public long UserId { get; }
    public string Name { get; }
    public string Path { get; }
    public string AuthorName { get; }
    public int Year { get; }
    public long SizeBytes { get; }
    public TimeSpan Duration { get; }
    public DateTime UploadedAt { get; }
    public long GenreId { get; }
    public Genre? Genre { get; set; }
}