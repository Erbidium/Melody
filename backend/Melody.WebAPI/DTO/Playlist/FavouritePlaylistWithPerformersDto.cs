namespace Melody.WebAPI.DTO.Playlist;

public class FavouritePlaylistWithPerformersDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public long AuthorId { get; set; }
    public bool IsFavourite { get; set; }
    public List<string> PerformersNames { get; set; }
}