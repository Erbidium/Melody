UPDATE Users
SET IsBanned = @IsBanned
WHERE Id = @UserId AND IsDeleted = 0;