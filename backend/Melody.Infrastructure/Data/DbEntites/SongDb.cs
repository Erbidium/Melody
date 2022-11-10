namespace Melody.Infrastructure.Data.Records;

public record SongDb(long Id, string Name, string Path, string AuthorName, int Year, long GenreId, bool IsDeleted);