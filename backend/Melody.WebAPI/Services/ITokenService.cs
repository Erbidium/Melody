using Melody.Infrastructure.Auth.Models;
using System.Security.Claims;
using Melody.Infrastructure.Data.DbEntites;

namespace Melody.WebAPI.Services;

public interface ITokenService
{
    Task<string> GenerateAccessToken(string email);
    Task<string> GenerateRefreshToken(string email, bool isExpired = false);
    Task<(string accessToken, string refreshToken)> CreateAccessTokenAndRefreshToken(string email);

    UserToken GetCurrentUser(ClaimsIdentity identity);
    Task<(string accessToken, string refreshToken)> GetAccessTokenAndUpdatedRefreshToken(string refreshToken);
}