using Melody.WebAPI.DTO.Song;

namespace Melody.WebAPI.DTO.Playlist;

public class PlaylistDto
{
    public long Id { get; set; }
    public string Name { get; }
    public long AuthorId { get; }
    public List<SongDto> Songs { get; set; } = new();
}