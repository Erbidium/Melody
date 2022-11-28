namespace Melody.WebAPI.DTO.Playlist;

public class UpdatePlaylistDto
{
    public string Name { get; set; }
    public long[] SongIds { get; set; }
}