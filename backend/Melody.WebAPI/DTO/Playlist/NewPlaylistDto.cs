namespace Melody.WebAPI.DTO.Playlist;

public class NewPlaylistDto
{
    public string Name { get; set; }
    public long[] SongIds { get; set; }
}