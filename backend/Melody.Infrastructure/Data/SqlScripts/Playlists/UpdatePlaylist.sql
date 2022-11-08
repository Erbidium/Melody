UPDATE Playlists
SET Name = @Name, Link = @Link, AuthorId = @AuthorId
WHERE Id = @Id AND IsDeleted = 0