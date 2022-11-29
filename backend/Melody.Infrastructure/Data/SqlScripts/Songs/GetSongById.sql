SELECT Id,
       UserId,
       UploadedAt,
       SizeBytes,
       Name,
       Path,
       AuthorName, Year, GenreId, Duration, IsDeleted
FROM Songs
WHERE Id = @Id
  AND IsDeleted = 0
