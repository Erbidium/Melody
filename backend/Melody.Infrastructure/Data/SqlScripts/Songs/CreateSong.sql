INSERT INTO Songs (UserId, Name, Path, AuthorName, Year, SizeBytes, UploadedAt, GenreId)
    OUTPUT Inserted.Id
VALUES (@UserId, @Name, @Path, @AuthorName, @Year, @SizeBytes, @UploadedAt, @GenreId)