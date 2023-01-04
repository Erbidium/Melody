using Melody.Infrastructure.Data.DbEntites;

namespace Melody.Infrastructure.Data.Interfaces;

public interface IUserRepository : Core.Interfaces.IUserRepository
{
    public Task<bool> CreateAsync(UserIdentity user);
    public Task<UserIdentity> FindByIdAsync(long userId);
    public Task<UserIdentity> FindByNameAsync(string normalizedUserName);
    public Task<UserIdentity> FindByEmailAsync(string normalizedEmail);
    public Task<bool> UpdateAsync(UserIdentity user);
    Task<bool> UpdateAsync(UserIdentity user, IList<UserRole> roles);
    public Task<IEnumerable<RoleIdentity>> GetRolesAsync(long userId);
    public Task<UserRole> FindUserRoleAsync(long userId, long roleId);
    public Task<IEnumerable<UserIdentity>> GetUsersInRoleAsync(string roleName);
}