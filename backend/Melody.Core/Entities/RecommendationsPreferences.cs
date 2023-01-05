using Ardalis.GuardClauses;

namespace Melody.Core.Entities;

public class RecommendationsPreferences
{
    public RecommendationsPreferences(long userId, long genreId, string? authorName, int? startYear, int? endYear,
        int? averageDurationInMinutes)
    {
        UserId = Guard.Against.Negative(userId, nameof(UserId));
        GenreId = Guard.Against.Negative(genreId, nameof(GenreId));
        AuthorName = string.IsNullOrEmpty(authorName?.Trim()) ? null : authorName;
        StartYear = startYear;
        EndYear = endYear;
        AverageDurationInMinutes = averageDurationInMinutes;
    }

    public long UserId { get; }
    public string? AuthorName { get; }
    public int? StartYear { get; }
    public int? EndYear { get; }
    public long GenreId { get; }
    public int? AverageDurationInMinutes { get; }
}