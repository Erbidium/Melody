namespace Melody.Infrastructure.Data.Entities;

public class Playlist
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Link { get; set; }
    public int AuthorId { get; set; }
    public bool IsDeleted { get; set; }
}
