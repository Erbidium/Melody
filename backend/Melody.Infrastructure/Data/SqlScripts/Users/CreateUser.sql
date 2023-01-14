INSERT INTO Users (UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, PhoneNumber)
OUTPUT Inserted.Id
VALUES (@UserName, @NormalizedUserName, @Email, @NormalizedEmail, @EmailConfirmed, @PasswordHash, @PhoneNumber)
