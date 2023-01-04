UPDATE Songs
SET IsDeleted = 1
WHERE Id = @Id
  AND UserId = @UserId
  AND IsDeleted = 0