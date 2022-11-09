namespace Melody.Core.Entities;

public class UserIdentity
{
    public long Id { get; set; }
    public string UserName { get; set; }
    public string NormalizedUserName { get; set; }
    public string Email { get; set; }
    public string NormalizedEmail { get; set; }
    public string EmailConfirmed { get; set; }
    public string PasswordHash { get; set; }
    public string PhoneNumber { get; set; }
    public long RoleId { get; set; }
    public bool IsBanned { get; set; }
    public bool IsDeleted { get; set; }
}
