using Melody.SharedKernel;

namespace Melody.Core.ValueObjects;

public class UserRoles : ValueObject
{
    public UserRoles(long userId, IEnumerable<string> roles)
    {
        UserId = userId;
        Roles = roles.ToList().AsReadOnly();
    }
    public long UserId { get; }
    public IReadOnlyCollection<string> Roles { get; }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return UserId;
        foreach (var role in Roles.OrderBy(role => role))
        {
            yield return role;
        }
    }
}