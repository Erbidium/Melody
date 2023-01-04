using Melody.Core.Entities;

namespace Melody.Core.Interfaces;

public interface IUserRepository
{
    Task<IReadOnlyCollection<User>> GetUsersWithoutAdministratorRole(string searchText, int page = 1, int pageSize = 10);
    Task<bool> SetUserBannedStatus(bool isBanned, long userId);
    Task<bool> DeleteAsync(long userId);
    Task<bool> CreateOrUpdateUserRecommendationsPreferences(RecommendationsPreferences recommendationsPreferences);
    Task<RecommendationsPreferences?> GetUserRecommendationsPreferences(long userId);
}