namespace Melody.Infrastructure.Data.Records;

public class UserIdentity
{
    public long Id { get; set; }
    public string UserName { get; set; }
    public string NormalizedUserName { get; set; }
    public string Email { get; set; }
    public string NormalizedEmail { get; set; }
    public bool EmailConfirmed { get; set; }
    public string PasswordHash { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsBanned { get; set; }
    public bool IsDeleted { get; set; }
}