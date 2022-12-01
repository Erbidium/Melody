SELECT p.Id,
       p.Name,
       p.AuthorId,
       p.IsDeleted,
       ps.Id,
       ps.UserId,
       ps.UploadedAt,
       ps.SizeBytes,
       ps.Name,
       ps.Path,
       ps.AuthorName,
       ps.Year,
       ps.GenreId,
       ps.Duration,
       ps.IsDeleted
FROM Playlists p
LEFT JOIN
    (
        SELECT
            s.Id,
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
            ps.PlaylistId
        FROM Songs s
        INNER JOIN PlaylistSongs ps ON ps.SongId = s.Id
        WHERE s.IsDeleted = 0
    ) ps ON ps.PlaylistId = p.Id
WHERE p.IsDeleted = 0 AND AuthorId = @UserId