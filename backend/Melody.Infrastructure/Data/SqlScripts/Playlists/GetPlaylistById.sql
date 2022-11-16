SELECT Id, Name, Link, AuthorId, IsDeleted
FROM Playlists
WHERE Id = @Id
  AND IsDeleted = 0