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
       CONVERT(BIT, IIF(fs.SongId IS NULL, 0, 1)) as IsFavourite,
       g.Id,
       g.Name
FROM Songs s
         INNER JOIN Genres g ON s.GenreId = g.Id
         INNER JOIN Users u ON u.Id = s.UserId
         LEFT JOIN
     (SELECT SongId
      FROM FavouriteSongs fs
      WHERE fs.UserId = @UserId) fs ON fs.SongId = s.Id
WHERE s.IsDeleted = 0
  AND u.IsDeleted = 0
  AND u.IsBanned = 0
  AND s.Id IN @Ids
ORDER BY s.UploadedAt DESC