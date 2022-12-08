WITH PlaylistSongsIds AS
(
    SELECT s.Id
    FROM Songs s
    INNER JOIN PlaylistSongs ps ON ps.SongId = s.Id
    INNER JOIN Playlists p ON ps.PlaylistId = p.Id
    WHERE s.IsDeleted = 0 AND p.IsDeleted = 0 AND p.Id = @PlaylistId
)
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
WHERE IsDeleted = 0 AND UserId = @UserId AND Id NOT IN (Select Id FROM PlaylistSongsIds)
UNION
SELECT Id,
       Songs.UserId,
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
INNER JOIN FavouriteSongs fs ON fs.SongId = Id
WHERE IsDeleted = 0 AND fs.UserId = @UserId AND Id NOT IN (Select Id FROM PlaylistSongsIds)