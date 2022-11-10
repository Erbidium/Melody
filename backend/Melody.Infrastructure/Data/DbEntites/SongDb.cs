namespace Melody.Infrastructure.Data.Records;

public record SongDb(long Id, long UserId, DateOnly UploadedAt, long SizeBytes, string Name, string GeneratedName, string Path, string AuthorName, int Year, long GenreId, bool IsDeleted);