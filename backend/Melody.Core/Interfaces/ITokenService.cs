using LanguageExt.Common;

namespace Melody.Core.Interfaces;

public interface ITokenService
{
    Task<string> GenerateRefreshToken(string email, bool isExpired = false);
    Task<Result<(string accessToken, string refreshToken)>> CreateAccessTokenAndRefreshToken(string email, string password);
    Task<Result<(string accessToken, string refreshToken)>> GetAccessTokenAndUpdatedRefreshToken(string refreshToken);
}