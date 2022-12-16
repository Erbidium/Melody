SELECT s.Id,
       s.UserId,
       UploadedAt,
       SizeBytes,
       s.Name,
       Path,
       AuthorName,
       Year,
       GenreId,
       Duration,
       IsDeleted,
       g.Id,
       g.Name
FROM Songs s
         INNER JOIN Genres g ON s.GenreId = g.Id
         INNER JOIN FavouriteSongs fs ON fs.SongId = s.Id
WHERE s.IsDeleted = 0
  AND fs.UserId = @UserId
ORDER BY s.UploadedAt DESC