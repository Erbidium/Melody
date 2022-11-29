namespace Melody.WebAPI.DTO.Playlist;

public class PlaylistWithPerformersDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public long AuthorId { get; set; }
    public List<string> PerformersNames { get; set; }
}