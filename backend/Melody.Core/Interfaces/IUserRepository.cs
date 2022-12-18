using Melody.Core.Entities;

namespace Melody.Core.Interfaces;

public interface IUserRepository
{
    Task<IReadOnlyCollection<User>> GetUsersWithoutAdministratorRole();
    Task<bool> SetUserBannedStatus(bool isBanned, long userId);
}