INSERT INTO Songs (Name, Path, AuthorName, Year, GenreId)
OUTPUT Inserted.Id
VALUES (@Name, @Path, @AuthorName, @Year, @GenreId)