SELECT Roles.*
FROM Roles
INNER JOIN UserRoles ON UserRoles.RoleId = Roles.Id
INNER JOIN Users ON UserRoles.UserId = Users.Id
WHERE Users.Id = @UserId;