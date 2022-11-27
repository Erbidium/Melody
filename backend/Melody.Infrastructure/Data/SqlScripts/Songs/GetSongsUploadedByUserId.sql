SELECT Songs.Id,
       UserId,
       UploadedAt,
       SizeBytes,
       Songs.Name,
       Path,
       AuthorName,
       Year,
       GenreId,
       Duration,
       IsDeleted,
       Genres.Id,
       Genres.Name
FROM Songs
INNER JOIN Genres
ON Songs.GenreId = Genres.Id
WHERE IsDeleted = 0 AND UserId = @UserId