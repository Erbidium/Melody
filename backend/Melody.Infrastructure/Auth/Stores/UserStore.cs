using Melody.Infrastructure.Data.DbEntites;
using Melody.Infrastructure.Data.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Melody.Infrastructure.Auth.Stores;

public class UserStore : IUserStore<UserIdentity>, IUserRoleStore<UserIdentity>, IUserEmailStore<UserIdentity>,
    IUserPasswordStore<UserIdentity>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRepository _userRepository;

    private bool disposedValue;

    public UserStore(IUserRepository userRepository, IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    private IList<UserRole> UserRoles { get; set; } = new List<UserRole>();

    public Task SetEmailAsync(UserIdentity user, string email, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        user.ThrowIfNull(nameof(user));
        user.Email = email;
        return Task.CompletedTask;
    }

    public Task<string> GetEmailAsync(UserIdentity user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        user.ThrowIfNull(nameof(user));
        return Task.FromResult(user.Email);
    }

    public Task<bool> GetEmailConfirmedAsync(UserIdentity user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        user.ThrowIfNull(nameof(user));
        return Task.FromResult(user.EmailConfirmed);
    }

    public Task SetEmailConfirmedAsync(UserIdentity user, bool confirmed, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        user.ThrowIfNull(nameof(user));
        user.EmailConfirmed = confirmed;
        return Task.CompletedTask;
    }

    public Task<UserIdentity> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return _userRepository.FindByEmailAsync(normalizedEmail);
    }

    public Task<string> GetNormalizedEmailAsync(UserIdentity user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        user.ThrowIfNull(nameof(user));
        return Task.FromResult(user.NormalizedEmail);
    }

    public Task SetNormalizedEmailAsync(UserIdentity user, string normalizedEmail, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        user.ThrowIfNull(nameof(user));
        user.NormalizedEmail = normalizedEmail;
        return Task.CompletedTask;
    }

    public Task SetPasswordHashAsync(UserIdentity user, string passwordHash, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        user.ThrowIfNull(nameof(user));
        user.PasswordHash = passwordHash;
        return Task.CompletedTask;
    }

    public Task<string> GetPasswordHashAsync(UserIdentity user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        user.ThrowIfNull(nameof(user));
        return Task.FromResult(user.PasswordHash);
    }

    public Task<bool> HasPasswordAsync(UserIdentity user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        user.ThrowIfNull(nameof(user));
        return Task.FromResult(!string.IsNullOrWhiteSpace(user.PasswordHash));
    }

    public async Task AddToRoleAsync(UserIdentity user, string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        user.ThrowIfNull(nameof(user));
        if (string.IsNullOrEmpty(roleName))
            throw new ArgumentException($"Parameter {nameof(roleName)} cannot be null or empty.");

        var roleEntity = await FindRoleAsync(roleName, cancellationToken);
        if (roleEntity == null) throw new InvalidOperationException($"Role '{roleName}' was not found.");

        var userRoles = (await _userRepository.GetRolesAsync(user.Id))?.Select(x => new UserRole
        {
            UserId = user.Id,
            RoleId = x.Id
        }).ToList() ?? new List<UserRole>();
        UserRoles = userRoles;
        UserRoles.Add(new UserRole { UserId = user.Id, RoleId = roleEntity.Id });
    }

    public async Task RemoveFromRoleAsync(UserIdentity user, string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        user.ThrowIfNull(nameof(user));
        if (string.IsNullOrEmpty(roleName)) throw new ArgumentException(nameof(roleName));

        var roleEntity = await FindRoleAsync(roleName, cancellationToken);
        if (roleEntity != null)
        {
            var userRoles = (await _userRepository.GetRolesAsync(user.Id))?.Select(x => new UserRole
            {
                UserId = user.Id,
                RoleId = x.Id
            }).ToList() ?? new List<UserRole>();
            UserRoles = userRoles;
            var userRole = await FindUserRoleAsync(user.Id, roleEntity.Id, cancellationToken);
            if (userRole != null) UserRoles.Remove(userRole);
        }
    }

    public async Task<IList<string>> GetRolesAsync(UserIdentity user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        user.ThrowIfNull(nameof(user));
        var userRoles = await _userRepository.GetRolesAsync(user.Id);
        return userRoles.Select(x => x.Name).ToList();
    }

    public async Task<bool> IsInRoleAsync(UserIdentity user, string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        user.ThrowIfNull(nameof(user));
        if (string.IsNullOrEmpty(roleName)) throw new ArgumentException(null, nameof(roleName));

        var role = await FindRoleAsync(roleName, cancellationToken);
        if (role != null)
        {
            var userRole = await FindUserRoleAsync(user.Id, role.Id, cancellationToken);
            return userRole != null;
        }

        return false;
    }

    public async Task<IList<UserIdentity>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (string.IsNullOrEmpty(roleName)) throw new ArgumentNullException(nameof(roleName));

        var role = await FindRoleAsync(roleName, cancellationToken);
        var users = new List<UserIdentity>();
        if (role != null) users = (await _userRepository.GetUsersInRoleAsync(roleName)).ToList();

        return users;
    }

    public async Task<IdentityResult> CreateAsync(UserIdentity user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        user.ThrowIfNull(nameof(user));
        var created = await _userRepository.CreateAsync(user);
        return created
            ? IdentityResult.Success
            : IdentityResult.Failed(new IdentityError
            {
                Code = string.Empty,
                Description = $"UserIdentity '{user.UserName}' could not be created."
            });
    }

    public async Task<IdentityResult> DeleteAsync(UserIdentity user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        user.ThrowIfNull(nameof(user));
        var deleted = await _userRepository.DeleteAsync(user.Id);
        return deleted
            ? IdentityResult.Success
            : IdentityResult.Failed(new IdentityError
            {
                Code = string.Empty,
                Description = $"UserIdentity '{user.UserName}' could not be deleted."
            });
    }

    public async Task<UserIdentity> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var id = ConvertIdFromString(userId);
        var user = await _userRepository.FindByIdAsync(id);
        return user;
    }

    public async Task<UserIdentity> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var user = await _userRepository.FindByNameAsync(normalizedUserName);
        return user;
    }

    public Task<string> GetNormalizedUserNameAsync(UserIdentity user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        user.ThrowIfNull(nameof(user));
        return Task.FromResult(user.NormalizedUserName);
    }

    public Task<string> GetUserIdAsync(UserIdentity user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        user.ThrowIfNull(nameof(user));
        return Task.FromResult(user.Id.ToString());
    }

    public Task<string> GetUserNameAsync(UserIdentity user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        user.ThrowIfNull(nameof(user));
        return Task.FromResult(user.UserName);
    }

    public Task SetNormalizedUserNameAsync(UserIdentity user, string normalizedName,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (user == null) throw new ArgumentNullException(nameof(user));
        if (normalizedName == null) throw new ArgumentNullException(nameof(normalizedName));

        user.NormalizedUserName = normalizedName;
        return Task.FromResult<object>(null);
    }

    public Task SetUserNameAsync(UserIdentity user, string userName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (user == null) throw new ArgumentNullException(nameof(user));
        if (userName == null) throw new ArgumentNullException(nameof(userName));

        user.UserName = userName;
        return Task.FromResult<object>(null);
    }

    public async Task<IdentityResult> UpdateAsync(UserIdentity user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        user.ThrowIfNull(nameof(user));
        var updated = await _userRepository.UpdateAsync(user, UserRoles);
        return updated
            ? IdentityResult.Success
            : IdentityResult.Failed(new IdentityError
            {
                Code = string.Empty,
                Description = $"UserIdentity '{user.UserName}' could not be deleted."
            });
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private long ConvertIdFromString(string userId)
    {
        return long.Parse(userId);
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

    protected Task<RoleIdentity> FindRoleAsync(string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var findRoleTask = _roleRepository.FindByNameAsync(roleName);
        return findRoleTask;
    }

    protected async Task<UserRole> FindUserRoleAsync(long userId, long roleId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var userRole = await _userRepository.FindUserRoleAsync(userId, roleId);
        return userRole;
    }
}