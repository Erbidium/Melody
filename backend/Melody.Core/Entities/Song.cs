using Ardalis.GuardClauses;
using Melody.SharedKernel.Interfaces;

namespace Melody.Core.Entities;

public class Song : EntityBase<long>
{
    public Song(long userId, string name, string path, string authorName, int year, long genreId, long sizeBytes,
        DateTime uploadedAt, TimeSpan duration)
    {
        UserId = Guard.Against.Negative(userId, nameof(UserId));
        Name = Guard.Against.NullOrWhiteSpace(name, nameof(Name));
        Path = Guard.Against.NullOrWhiteSpace(path, nameof(Path));
        AuthorName = Guard.Against.NullOrWhiteSpace(authorName, nameof(AuthorName));
        Year = Guard.Against.NegativeOrZero(year, nameof(Year));
        GenreId = Guard.Against.Negative(genreId, nameof(GenreId));
        SizeBytes = Guard.Against.NegativeOrZero(sizeBytes, nameof(SizeBytes));
        UploadedAt = Guard.Against.Default(uploadedAt, nameof(UploadedAt));
        Duration = Guard.Against.NegativeOrZero(duration, nameof(Duration));
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