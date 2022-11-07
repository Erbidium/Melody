UPDATE Songs
SET Name = @Name, Path = @Path, AuthorName = @AuthorName, Year = @Year, GenreId = @GenreId
WHERE Id = @Id AND IsDeleted = 0