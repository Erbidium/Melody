UPDATE Users
SET UserName           = @UserName,
    NormalizedUserName = @NormalizedUserName,
    Email              = @Email,
    NormalizedEmail    = @NormalizedEmail,
    EmailConfirmed     = @EmailConfirmed,
    PasswordHash       = @PasswordHash,
    PhoneNumber        = @PhoneNumber,
    IsBanned           = @IsBanned,
    IsDeleted          = @IsDeleted
WHERE Id = @Id
  AND Users.IsDeleted = 0;