using Melody.Infrastructure.Auth.Models;
using Melody.Infrastructure.Data.Records;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Melody.WebAPI.Services;

public interface ITokenService
{
    string GenerateAccessToken(UserIdentity user, IList<string> roles);
    string GenerateRefreshToken(UserIdentity user);
    UserToken? GetCurrentUser(ClaimsIdentity identity);
    TokenValidationParameters GetValidationParameters();
}