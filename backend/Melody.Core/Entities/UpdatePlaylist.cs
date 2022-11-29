namespace Melody.Core.Entities;

public class UpdatePlaylist
{
    public long Id { get; set; }
    public string Name { get; set; }
    public long[] SongIds { get; set; }
}