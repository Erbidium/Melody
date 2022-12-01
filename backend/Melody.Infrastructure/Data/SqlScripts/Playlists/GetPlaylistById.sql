SELECT
    p.Id,
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
    ps.IsDeleted,
    ps.GenreId as Id,
    ps.GenreName as Name
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
            g.Name as GenreName,
            ps.PlaylistId
        FROM
        Songs s
        INNER JOIN PlaylistSongs ps ON ps.SongId = s.Id
        INNER JOIN Genres g ON s.GenreId = g.Id
        WHERE s.IsDeleted = 0
    ) ps ON ps.PlaylistId = p.Id
WHERE p.Id = @Id AND p.IsDeleted = 0