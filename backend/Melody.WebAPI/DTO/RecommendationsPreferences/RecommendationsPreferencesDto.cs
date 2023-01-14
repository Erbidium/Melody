namespace Melody.WebAPI.DTO.RecommendationsPreferences;

public class RecommendationsPreferencesDto
{
    public string? AuthorName { get; set; }
    public int? StartYear { get; set; }
    public int? EndYear { get; set; }
    public long GenreId { get; set; }
    public int? AverageDurationInMinutes { get; set; }
}