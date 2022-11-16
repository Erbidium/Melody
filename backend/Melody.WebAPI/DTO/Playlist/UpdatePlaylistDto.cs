namespace Melody.WebAPI.DTO.Playlist;

public class UpdatePlaylistDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Link { get; set; }
    public long AuthorId { get; set; }
}