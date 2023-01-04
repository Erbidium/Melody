namespace Melody.Infrastructure.Data.DbEntites;

public class RecommendationsPreferencesDb
{
    public long UserId { get; set; }
    public string? AuthorName { get; set; }
    public int? StartYear { get; set; }
    public int? EndYear { get; set; }
    public long GenreId { get; set; }
    public int? AverageDurationInMinutes { get; set; }
}