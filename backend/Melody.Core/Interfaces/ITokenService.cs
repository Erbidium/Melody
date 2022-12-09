using System.Security.Claims;
using Melody.Core.ValueObjects;

namespace Melody.Core.Interfaces;

public interface ITokenService
{
    Task<string> GenerateRefreshToken(string email, bool isExpired = false);
    Task<(string? accessToken, string? refreshToken)> CreateAccessTokenAndRefreshToken(string email, string password);

    UserRoles GetCurrentUser(ClaimsIdentity identity);
    Task<(string? accessToken, string? refreshToken)> GetAccessTokenAndUpdatedRefreshToken(string refreshToken);
}