namespace Melody.Core.Entities;

public class UserWithRoles : User
{
    public UserWithRoles(IEnumerable<string> roles, string userName, string email, string phoneNumber, bool isBanned = false)
        : base(userName, email, phoneNumber, isBanned)
    {
        Roles = roles.ToList().AsReadOnly();
    }
    
    public IReadOnlyList<string> Roles { get; }
}