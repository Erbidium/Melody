SELECT Id, UserId, RefreshToken
FROM UserRefreshTokens
WHERE UserId = @UserId;