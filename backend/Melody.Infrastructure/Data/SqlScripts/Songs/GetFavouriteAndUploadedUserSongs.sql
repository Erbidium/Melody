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
WHERE IsDeleted = 0
  AND UserId = @UserId