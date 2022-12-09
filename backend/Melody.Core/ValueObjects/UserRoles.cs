namespace Melody.Core.ValueObjects;

public class UserRoles
{
    public UserRoles(long userId, IEnumerable<string> roles)
    {
        UserId = userId;
        Roles = roles.ToList().AsReadOnly();
    }
    public long UserId { get; }
    public IReadOnlyCollection<string> Roles { get; }
}