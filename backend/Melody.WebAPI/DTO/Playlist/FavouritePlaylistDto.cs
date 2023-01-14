using Melody.WebAPI.DTO.Song;

namespace Melody.WebAPI.DTO.Playlist;

public class FavouritePlaylistDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public long AuthorId { get; set; }
    public bool IsFavourite { get; set; }
    public List<SongInPlaylistDto> Songs { get; set; } = new();
}