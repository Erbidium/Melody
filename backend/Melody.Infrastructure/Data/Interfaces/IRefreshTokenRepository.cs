using Melody.Infrastructure.Data.DbEntites;

namespace Melody.Infrastructure.Data.Interfaces;

public interface IRefreshTokenRepository
{
    Task<bool> CreateOrUpdateAsync(string token, long userId);
    Task<bool> DeleteAsync(string Token);
    Task<RefreshTokenDb> FindAsync(string token);
}