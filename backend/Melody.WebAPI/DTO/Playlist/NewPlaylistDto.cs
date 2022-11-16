namespace Melody.WebAPI.DTO.Playlist;

public class NewPlaylistDto
{
    public string Name { get; set; }
    public string Link { get; set; }
    public long AuthorId { get; set; }
}