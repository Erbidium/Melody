UPDATE UserRefreshTokens
SET RefreshToken = @Token
WHERE UserId = @Id;