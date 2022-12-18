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
       g.Id,
       g.Name
FROM Songs s
         INNER JOIN Genres g ON s.GenreId = g.Id
         INNER JOIN FavouriteSongs fs ON fs.SongId = s.Id
         INNER JOIN Users u ON u.Id = fs.UserId
WHERE s.IsDeleted = 0
  AND fs.UserId = @UserId
  AND u.IsDeleted = 0
  AND u.IsBanned = 0
ORDER BY s.UploadedAt DESC