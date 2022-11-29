INSERT INTO Playlists (Name, AuthorId)
    OUTPUT Inserted.Id
VALUES (@Name, @AuthorId)