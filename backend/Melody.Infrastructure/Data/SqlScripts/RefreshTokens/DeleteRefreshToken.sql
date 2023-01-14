DELETE
FROM UserRefreshTokens
WHERE RefreshToken = @Token;