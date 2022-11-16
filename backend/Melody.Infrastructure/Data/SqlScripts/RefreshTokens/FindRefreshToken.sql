SELECT Id, UserId, RefreshToken
FROM UserRefreshTokens
WHERE RefreshToken = @Token;