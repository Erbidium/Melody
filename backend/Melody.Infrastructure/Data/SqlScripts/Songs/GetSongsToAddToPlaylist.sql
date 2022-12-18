WITH PlaylistSongsIds AS
         (SELECT s.Id
          FROM Songs s
                   INNER JOIN PlaylistSongs ps ON ps.SongId = s.Id
                   INNER JOIN Playlists p ON ps.PlaylistId = p.Id
          WHERE s.IsDeleted = 0
            AND p.IsDeleted = 0
            AND p.Id = @PlaylistId)
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
       s.IsDeleted
FROM Songs s
INNER JOIN Users u ON u.Id = s.UserId
WHERE s.IsDeleted = 0
  AND s.UserId = @UserId
  AND s.Id NOT IN (Select Id FROM PlaylistSongsIds)
  AND u.IsDeleted = 0
  AND u.IsBanned = 0
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
       s.IsDeleted
FROM Songs s
         INNER JOIN FavouriteSongs fs ON fs.SongId = Id
         INNER JOIN Users u ON u.Id = fs.UserId
WHERE s.IsDeleted = 0
  AND fs.UserId = @UserId
  AND s.Id NOT IN (Select Id FROM PlaylistSongsIds)
  AND u.IsDeleted = 0
  AND u.IsBanned = 0