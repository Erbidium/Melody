namespace Melody.Infrastructure.Data.DbEntites;

public class SongDb
{
    public long Id { get; init; }
    public long UserId { get; init; }
    public DateTime UploadedAt { get; init; }
    public long SizeBytes { get; init; }
    public string Name { get; init; }
    public string Path { get; init; }
    public string AuthorName { get; init; }
    public int Year { get; init; }
    public long GenreId { get; init; }
    public TimeSpan Duration { get; init; }
    public bool IsDeleted { get; init; }

    public GenreDb Genre { get; set; }
}