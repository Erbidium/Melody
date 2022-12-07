SELECT p.Id,
       p.Name,
       p.AuthorId,
       p.IsDeleted,
       ps.Id,
       ps.UploadedAt,
       ps.Name,
       ps.AuthorName,
       ps.GenreId,
       ps.Duration,
       ps.IsDeleted,
       ps.IsFavourite
FROM Playlists p
LEFT JOIN
    (
        SELECT
            s.Id,
            s.UploadedAt,
            s.Name,
            s.AuthorName,
            s.GenreId,
            s.Duration,
            s.IsDeleted,
            ps.PlaylistId,
            CONVERT(BIT, IIF(fs.SongId IS NULL, 0, 1)) as IsFavourite
        FROM Songs s
        INNER JOIN PlaylistSongs ps ON ps.SongId = s.Id
        Left JOIN
             (
                 SELECT SongId
                 FROM FavouriteSongs fs
                 WHERE fs.UserId = @UserId
             ) fs ON fs.SongId = s.Id
        WHERE s.IsDeleted = 0
    ) ps ON ps.PlaylistId = p.Id
WHERE p.IsDeleted = 0 AND p.AuthorId = @UserId