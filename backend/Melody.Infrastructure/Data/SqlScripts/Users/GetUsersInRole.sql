SELECT Users.*
FROM Users
INNER JOIN Roles ON Users.RoleId = Roles.Id
WHERE Roles.NormalizedName = @NormalizedName;