using System.Security.Claims;

namespace Melody.WebAPI.Extensions;

public static class HttpUserExtensions
{
    public static long GetId(this ClaimsPrincipal user)
    {
        var stringId = user.Claims
            .FirstOrDefault(claim => claim.Type == "UserId")?
            .Value;

        if (stringId is null) throw new KeyNotFoundException("UserId not found in user's claims");

        return long.Parse(stringId);
    }

    public static IReadOnlyCollection<string> GetUserRoles(this ClaimsPrincipal user)
    {
        var roles = user.Claims
            .Where(o => o.Type == ClaimTypes.Role)
            .Select(r => r.Value)
            .ToList()
            .AsReadOnly();

        if (roles.Count == 0) throw new KeyNotFoundException("Roles are not found in user's claims");

        return roles;
    }
}