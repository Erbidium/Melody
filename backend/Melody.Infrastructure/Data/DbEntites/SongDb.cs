namespace Melody.Infrastructure.Data.Records;

public record SongDb(long Id, long UserId, DateTime UploadedAt, long SizeBytes, string Name, string Path,
    string AuthorName, int Year, long GenreId, TimeSpan Duration, bool IsDeleted);