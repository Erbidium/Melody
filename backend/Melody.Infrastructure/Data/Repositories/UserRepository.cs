using Melody.Core.Entities;
using Melody.Core.Interfaces;

namespace Melody.Infrastructure.Data.Repositories;

public class UserRepository : IUserRepository
{
    public Task<bool> CreateAsync(UserIdentity user)
    {
        throw new NotImplementedException();
    }

    public Task<bool> CreateUserRole(UserIdentity user, RoleIdentity role)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(long userId)
    {
        throw new NotImplementedException();
    }

    public Task<UserIdentity> FindByEmailAsync(string normalizedEmail)
    {
        throw new NotImplementedException();
    }

    public Task<UserIdentity> FindByIdAsync(long userId)
    {
        throw new NotImplementedException();
    }

    public Task<UserIdentity> FindByNameAsync(string normalizedUserName)
    {
        throw new NotImplementedException();
    }

    public Task<UserRole> FindUserRoleAsync(long userId, long roleId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<RoleIdentity>> GetRolesAsync(long userId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<UserIdentity>> GetUsersInRoleAsync(string roleName)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(UserIdentity user)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(UserIdentity user, IList<UserRole> roles)
    {
        throw new NotImplementedException();
    }
}
