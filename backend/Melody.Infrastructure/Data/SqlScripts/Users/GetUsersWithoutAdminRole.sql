IF ISNULL(@SearchText, '') = '' SET @SearchText = '""';

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
WHERE NOT EXISTS(
        SELECT u.Id
        FROM Users u
                 INNER JOIN UserRoles ur ON ur.UserId = u.Id
        WHERE ur.RoleId = 1
          AND ur.UserId = Users.Id
    )
  AND IsDeleted = 0
  AND (@SearchText = '""' OR CONTAINS (Users.UserName, @SearchText))
ORDER BY Users.UserName
OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;