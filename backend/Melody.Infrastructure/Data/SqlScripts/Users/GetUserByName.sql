SELECT Id,
       UserName,
       NormalizedUserName,
       Email,
       NormalizedEmail,
       EmailConfirmed,
       PasswordHash,
       PhoneNumber,
       IsBanned,
       IsDeleted
FROM Users
WHERE NormalizedUserName = @NormalizedUserName;