INSERT INTO Playlists (Name, Link, AuthorId)
    OUTPUT Inserted.Id
VALUES (@Name, @Link, @AuthorId)