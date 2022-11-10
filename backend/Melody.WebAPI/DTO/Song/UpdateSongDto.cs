namespace Melody.WebAPI.DTO.Song;

public class UpdateSongDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string AuthorName { get; set; }
    public int Year { get; set; }
    public long GenreId { get; set; }
}
