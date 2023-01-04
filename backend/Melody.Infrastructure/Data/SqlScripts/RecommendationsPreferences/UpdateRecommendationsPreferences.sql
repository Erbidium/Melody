UPDATE RecommendationsPreferences
SET
	AuthorName = @AuthorName,
	StartYear = @StartYear,
	EndYear = @EndYear,
	GenreId = @GenreId,
	AverageDurationInMinutes = @AverageDurationInMinutes
WHERE UserId = @UserId;