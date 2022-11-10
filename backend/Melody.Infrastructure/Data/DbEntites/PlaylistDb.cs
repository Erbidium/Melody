namespace Melody.Infrastructure.Data.Records;

public record PlaylistDb(long Id, string Name, string Link, long AuthorId, bool IsDeleted);
