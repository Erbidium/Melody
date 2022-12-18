﻿SELECT p.Id,
       p.Name,
       p.AuthorId,
       CONVERT(BIT, IIF(up.PlaylistId IS NULL, 0, 1)) as IsFavourite,
       p.IsDeleted,
       ps.Id,
       ps.UploadedAt,
       ps.Name,
       ps.AuthorName,
       ps.GenreId,
       ps.Duration,
       ps.IsDeleted,
       ps.IsFavourite,
       ps.GenreId                                     as Id,
       ps.GenreName                                   as Name
FROM Playlists p
         LEFT JOIN
     (SELECT s.Id,
             s.UploadedAt,
             s.Name,
             s.AuthorName,
             s.GenreId,
             s.Duration,
             s.IsDeleted,
             g.Name                                     as GenreName,
             ps.PlaylistId,
             CONVERT(BIT, IIF(fs.SongId IS NULL, 0, 1)) as IsFavourite
      FROM Songs s
               INNER JOIN PlaylistSongs ps ON ps.SongId = s.Id
               INNER JOIN Genres g ON s.GenreId = g.Id
               LEFT JOIN
           (SELECT SongId
            FROM FavouriteSongs fs
            WHERE fs.UserId = @UserId) fs ON fs.SongId = s.Id
      WHERE s.IsDeleted = 0) ps ON ps.PlaylistId = p.Id
         LEFT JOIN
     (SELECT PlaylistId
      FROM UserPlaylists up
      WHERE up.UserId = @UserId) up ON up.PlaylistId = p.Id
      INNER JOIN Users u ON u.Id = p.AuthorId
WHERE p.Id = @Id
  AND p.IsDeleted = 0
  AND u.IsDeleted = 0
  AND u.IsBanned = 0
ORDER BY ps.UploadedAt DESC