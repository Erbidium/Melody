using Melody.Infrastructure.Data.DbEntites;

namespace Melody.Infrastructure.Data.Interfaces;

public interface IRefreshTokenRepository
{
    Task<bool> CreateOrUpdateAsync(string token, long userId);
    Task<bool> DeleteByValueAsync(string token);
    Task<bool> DeleteByUserIdAsync(long userId);
    Task<RefreshTokenDb?> FindByTokenValueAsync(string token);
    Task<RefreshTokenDb?> FindByUserIdAsync(long userId);
}