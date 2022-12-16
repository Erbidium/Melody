SELECT Id,
       UserId,
       UploadedAt,
       SizeBytes,
       Name,
       Path,
       AuthorName,
       Year,
       GenreId,
       Duration,
       IsDeleted
FROM Songs
WHERE IsDeleted = 0
ORDER BY UploadedAt DESC