namespace Melody.Core.Entities;

public class CreatePlaylist
{
    public string Name { get; set; }
    public long AuthorId { get; set; }
    public long[] SongIds { get; set; }
}