SELECT Users.Id,
       Users.UserName,
       Users.NormalizedUserName,
       Users.Email,
       Users.NormalizedEmail,
       Users.EmailConfirmed,
       Users.PasswordHash,
       Users.PhoneNumber,
       Users.IsBanned,
       Users.IsDeleted
FROM Users
         INNER JOIN Roles ON Users.RoleId = Roles.Id
WHERE Roles.NormalizedName = @NormalizedName
  AND Users.IsDeleted = 0;