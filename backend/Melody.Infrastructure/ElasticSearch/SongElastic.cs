namespace Melody.Infrastructure.ElasticSearch;

public class SongElastic
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string AuthorName { get; set; }
    public int Year { get; set; }
    public int DurationInSeconds { get; set; }
    public DateTime UploadedAt { get; set; }
    public long GenreId { get; set; }
}