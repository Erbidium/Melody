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
UNION
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
FROM Songs s INNER JOIN Genres ON s.GenreId = Genres.Id
INNER JOIN Users u ON u.Id = s.UserId
WHERE s.IsDeleted = 0
  AND s.UserId = @UserId
  AND u.IsDeleted = 0
  AND u.IsBanned = 0