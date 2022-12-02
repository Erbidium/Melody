SELECT Id,
       UserId,
       UploadedAt,
       SizeBytes,
       Name,
       Path,
       AuthorName,
       Year,
       GenreId,
       Duration,
       IsDeleted
FROM Songs
WHERE IsDeleted = 0
  ANd Id NOT IN
  (
    SELECT s.Id
    FROM Songs s
    INNER JOIN PlaylistSongs ps ON ps.SongId = s.Id
    INNER JOIN Playlists p ON ps.PlaylistId = p.Id
    WHERE s.IsDeleted = 0 AND p.IsDeleted = 0 AND p.Id = @PlaylistId
  )