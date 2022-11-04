namespace Melody.Infrastructure.Data.Records;

public record SongRecord(long Id, string Name, string Path, string AuthorName, int Year, long GenreId, bool IsDeleted);