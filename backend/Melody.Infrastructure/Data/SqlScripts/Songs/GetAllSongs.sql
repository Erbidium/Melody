SELECT Songs.Id,
       UserId,
       UploadedAt,
       SizeBytes,
       Songs.Name,
       Songs.Path,
       Songs.AuthorName,
       Songs.Year,
       Songs.GenreId,
       Songs.Duration,
       Songs.IsDeleted,
       Genres.Id,
       Genres.Name
FROM Songs
INNER JOIN Genres ON Songs.GenreId = Genres.Id
INNER JOIN Users u ON u.Id = p.AuthorId
WHERE u.IsDeleted = 0
  AND s.IsDeleted = 0
ORDER BY Songs.UploadedAt DESC