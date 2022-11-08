namespace Melody.Infrastructure.Data.Records;

public record UserRecord(long Id, string Name, string Email, string PhoneNumber, long RoleId, bool IsBanned, bool IsDeleted);
