using Melody.WebAPI.DTO.Song;

namespace Melody.WebAPI.DTO.Playlist;

public class PlaylistDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public long AuthorId { get; set; }
    public List<SongInPlaylistDto> Songs { get; set; } = new();
}