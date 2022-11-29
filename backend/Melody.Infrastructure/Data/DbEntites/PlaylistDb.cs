namespace Melody.Infrastructure.Data.DbEntites;

public class PlaylistDb
{
    public long Id { get; set; }
    public string Name { get; set; }
    public long AuthorId { get; set; }
    public bool IsDeleted { get; set; }
    public List<SongDb> Songs { get; set; } = new List<SongDb>();
}