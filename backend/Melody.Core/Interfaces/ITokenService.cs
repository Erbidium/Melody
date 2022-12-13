namespace Melody.Core.Interfaces;

public interface ITokenService
{
    Task<string> GenerateRefreshToken(string email, bool isExpired = false);
    Task<(string? accessToken, string? refreshToken)> CreateAccessTokenAndRefreshToken(string email, string password);
    Task<(string? accessToken, string? refreshToken)> GetAccessTokenAndUpdatedRefreshToken(string refreshToken);
}