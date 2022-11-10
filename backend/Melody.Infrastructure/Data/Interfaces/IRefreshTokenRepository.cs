using Melody.Infrastructure.Data.DbEntites;

namespace Melody.Infrastructure.Data.Interfaces;

internal interface IRefreshTokenRepository
{
    Task<bool> CreateOrUpdateAsync(string token, long userId);
    Task<bool> DeleteAsync(string Token);
    Task<RefreshTokenDb> FindAsync(string token);
}
