SELECT Id, Name, Path, AuthorName, Year, GenreId, IsDeleted
FROM Songs
WHERE Id = @Id AND IsDeleted = 0
