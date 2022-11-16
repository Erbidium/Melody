SELECT UserId, RoleId
FROM UserRoles
WHERE UserId = @UserId
  AND RoleId = @RoleId;