namespace Melody.Infrastructure.Data.DbEntites;

public record PlaylistDb(long Id, string Name, long AuthorId, bool IsDeleted);