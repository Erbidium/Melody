namespace Melody.WebAPI.DTO.Song;

public class NewSongDto
{
    public string Name { get; set; }
    public string AuthorName { get; set; }
    public int Year { get; set; }
    public string Path { get; set; }
    public long SizeInBytes { get; set; }
    public long GenreId { get; set; }
}