UPDATE Songs
SET IsDeleted = 1
WHERE Id = @Id AND IsDeleted = 0