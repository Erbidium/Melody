namespace Melody.Infrastructure.Data.DbEntites;

public class FavouriteSongFromPlaylistDb
{
    public long Id { get; init; }
    public DateTime UploadedAt { get; init; }
    public string Name { get; init; }
    public string AuthorName { get; init; }
    public long GenreId { get; init; }
    public TimeSpan Duration { get; init; }
    public bool IsDeleted { get; init; }
    public GenreDb Genre { get; set; }
    public bool IsFavourite { get; set; }
}