using Melody.Infrastructure.Data.Records;

namespace Melody.Infrastructure.Data.Interfaces;

public interface IRoleRepository
{
    Task<bool> CreateAsync(RoleIdentity role);
    Task<bool> DeleteAsync(long roleId);
    Task<RoleIdentity> FindByIdAsync(long roleId);
    Task<RoleIdentity> FindByNameAsync(string normalizedName);
    Task<bool> UpdateAsync(RoleIdentity role);
}