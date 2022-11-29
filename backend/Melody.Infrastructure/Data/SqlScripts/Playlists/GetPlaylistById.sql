SELECT
    p.Id,
    p.Name,
    p.AuthorId,
    p.IsDeleted,
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
    s.IsDeleted
FROM Playlists p
INNER JOIN PlaylistSongs ps ON ps.PlaylistId = p.Id
INNER JOIN Songs s ON ps.SongId = s.Id
WHERE p.Id = @Id AND p.IsDeleted = 0 AND s.IsDeleted = 0