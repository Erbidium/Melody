namespace Melody.Infrastructure.Data.Records;

public record PlaylistRecord(long Id, string Name, string Link, long AuthorId, bool IsDeleted);
