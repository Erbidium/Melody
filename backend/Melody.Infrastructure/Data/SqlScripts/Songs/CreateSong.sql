INSERT INTO Songs (UserId, Name, Path, AuthorName, Year, SizeBytes, UploadedAt, GenreId, Duration)
OUTPUT Inserted.Id
VALUES (@UserId, @Name, @Path, @AuthorName, @Year, @SizeBytes, @UploadedAt, @GenreId, @Duration)