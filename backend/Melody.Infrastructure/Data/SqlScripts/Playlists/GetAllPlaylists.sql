SELECT Id, Name, AuthorId, IsDeleted
FROM Playlists p
INNER JOIN Users u ON u.Id = p.AuthorId
WHERE u.IsDeleted = 0
  AND u.IsBanned = 0
  AND p.IsDeleted = 0