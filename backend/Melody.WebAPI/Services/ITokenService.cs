using Melody.Infrastructure.Auth.Models;
using System.Security.Claims;
using Melody.Infrastructure.Data.DbEntites;

namespace Melody.WebAPI.Services;

public interface ITokenService
{
    Task<string> GenerateAccessToken(UserIdentity user);
    string GenerateRefreshToken(UserIdentity user, bool isExpired = false);
    UserToken GetCurrentUser(ClaimsIdentity identity);
    string? GetEmailFromRefreshToken(string refreshToken);
    Task<(string accessToken, string refreshToken)> GetAccessTokenAndUpdatedRefreshToken(string refreshToken);
}