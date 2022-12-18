UPDATE Users
SET IsBanned = @IsBanned
WHERE Id = @UserId;