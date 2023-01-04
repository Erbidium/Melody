namespace Melody.Infrastructure.Data.DbEntites;

public class FavouritePlaylistDb
{
    public long Id { get; set; }
    public string Name { get; set; }
    public long AuthorId { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsFavourite { get; set; }
    public List<FavouriteSongFromPlaylistDb> Songs { get; set; } = new();
}