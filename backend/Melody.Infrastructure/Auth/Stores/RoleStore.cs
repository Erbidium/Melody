using Melody.Infrastructure.Data.DbEntites;
using Melody.Infrastructure.Data.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Melody.Infrastructure.Auth.Stores;

public class RoleStore : IRoleStore<RoleIdentity>
{
    private bool disposedValue;
    private readonly IRoleRepository _roleRepository;

    public RoleStore(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<IdentityResult> CreateAsync(RoleIdentity role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        role.ThrowIfNull(nameof(role));
        var created = await _roleRepository.CreateAsync(role);
        return created
            ? IdentityResult.Success
            : IdentityResult.Failed(new IdentityError
            {
                Code = string.Empty,
                Description = $"Role '{role.Name}' could not be created."
            });
    }

    public async Task<IdentityResult> DeleteAsync(RoleIdentity role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        role.ThrowIfNull(nameof(role));
        var deleted = await _roleRepository.DeleteAsync(role.Id);
        return deleted
            ? IdentityResult.Success
            : IdentityResult.Failed(new IdentityError
            {
                Code = string.Empty,
                Description = $"Role '{role.Name}' could not be deleted."
            });
    }

    public async Task<RoleIdentity> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var id = long.Parse(roleId);
        return await _roleRepository.FindByIdAsync(id);
    }

    public async Task<RoleIdentity> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await _roleRepository.FindByNameAsync(normalizedRoleName);
    }

    public Task<string> GetNormalizedRoleNameAsync(RoleIdentity role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        role.ThrowIfNull(nameof(role));
        return Task.FromResult(role.NormalizedName);
    }

    public Task<string> GetRoleIdAsync(RoleIdentity role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        role.ThrowIfNull(nameof(role));
        return Task.FromResult(role.Id.ToString());
    }

    public Task<string> GetRoleNameAsync(RoleIdentity role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        role.ThrowIfNull(nameof(role));
        return Task.FromResult(role.Name);
    }

    public Task SetNormalizedRoleNameAsync(RoleIdentity role, string normalizedName,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        role.ThrowIfNull(nameof(role));
        role.NormalizedName = normalizedName;
        return Task.CompletedTask;
    }

    public Task SetRoleNameAsync(RoleIdentity role, string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        role.ThrowIfNull(nameof(role));
        role.Name = roleName;
        return Task.CompletedTask;
    }

    public async Task<IdentityResult> UpdateAsync(RoleIdentity role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        role.ThrowIfNull(nameof(role));
        var updated = await _roleRepository.UpdateAsync(role);
        return updated
            ? IdentityResult.Success
            : IdentityResult.Failed(new IdentityError
            {
                Code = string.Empty,
                Description = $"Role '{role.Name}' could not be updated."
            });
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}