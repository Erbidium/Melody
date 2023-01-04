SELECT s.Id,
       s.UserId,
       s.UploadedAt,
       s.SizeBytes,
       s.Name,
       s.Path,
       s.AuthorName,
       s.Year,
       s.GenreId,
       s.Duration,
       s.IsDeleted
FROM Songs s
INNER JOIN Users u ON u.Id = s.UserId
WHERE s.Id = @Id
  AND s.IsDeleted = 0
  AND u.IsDeleted = 0
  AND u.IsBanned = 0

