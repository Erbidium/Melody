SELECT s.Id,
       s.UserId,
       s.UploadedAt,
       s.SizeBytes,
       s.Name,
       s.Path,
       s.AuthorName,
       s.Year,
       s.GenreId,
       s.Duration,
       s.IsDeleted,
       Genres.Id,
       Genres.Name
FROM Songs s
INNER JOIN Genres ON s.GenreId = Genres.Id
WHERE s.IsDeleted = 0
  AND s.UserId = @UserId
ORDER BY s.UploadedAt DESC