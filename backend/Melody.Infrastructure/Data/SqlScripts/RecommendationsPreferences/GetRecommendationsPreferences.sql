SELECT
    UserId,
    AuthorName,
    StartYear,
    EndYear,
    GenreId,
    AverageDurationInMinutes
FROM RecommendationsPreferences
WHERE UserId = @UserId;