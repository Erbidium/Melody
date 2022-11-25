SELECT SUM(SizeBytes)
FROM Songs
WHERE UserId = @UserId
  AND IsDeleted = 0