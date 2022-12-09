using Melody.Infrastructure.Auth.Models;
using System.Security.Claims;
using Melody.Infrastructure.Data.DbEntites;

namespace Melody.WebAPI.Services;

public interface ITokenService
{
    string GenerateAccessToken(UserIdentity user, IList<string> roles);
    string GenerateRefreshToken(UserIdentity user, bool isExpired = false);
    UserToken GetCurrentUser(ClaimsIdentity identity);
    string? GetEmailFromRefreshToken(string refreshToken);
}