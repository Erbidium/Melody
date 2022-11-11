UPDATE Songs
SET UserId = @UserId, Name = @Name, Path = @Path, AuthorName = @AuthorName, Year = @Year, SizeBytes = @SizeBytes, UploadedAt = @UploadedAt, GenreId = @GenreId
WHERE Id = @Id AND IsDeleted = 0