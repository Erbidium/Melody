using Melody.WebAPI.DTO.Genre;

namespace Melody.WebAPI.DTO.Song;

public class SongInPlaylistDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string AuthorName { get; set; }
    public TimeSpan Duration { get; set; }
    public DateTime UploadedAt { get; set; }
    public long GenreId { get; set; }
    public GenreDto? Genre { get; set; }
    public bool IsFavourite { get; set; }
}