using Melody.Core.Entities;

namespace Melody.Core.Interfaces;

public interface IUserRepository
{
    public Task<bool> CreateAsync(UserIdentity user);
    public Task<bool> DeleteAsync(long userId);
    public Task<UserIdentity> FindByIdAsync(long userId);
    public Task<UserIdentity> FindByNameAsync(string normalizedUserName);
    public Task<UserIdentity> FindByEmailAsync(string normalizedEmail);
    public Task<bool> UpdateAsync(UserIdentity user);
    Task<bool> UpdateAsync(UserIdentity user, IList<TUserRole> roles, IList<TUserToken> tokens);
}
