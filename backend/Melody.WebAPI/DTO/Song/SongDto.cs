namespace Melody.WebAPI.DTO.Song;

public class SongDto
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
    public string AuthorName { get; set; }
    public int Year { get; set; }
    public long SizeBytes { get; set; }
    public TimeSpan Duration { get; set; }
    public DateTime UploadedAt { get; set; }
    public long GenreId { get; set; }
}