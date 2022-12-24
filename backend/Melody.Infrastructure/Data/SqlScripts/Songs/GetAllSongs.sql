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
INNER JOIN Users u ON u.Id = Songs.UserId
WHERE u.IsDeleted = 0
  AND Songs.IsDeleted = 0
ORDER BY Songs.UploadedAt DESC
OFFSET @Offset ROWS
FETCH NEXT @PageSize ROWS ONLY