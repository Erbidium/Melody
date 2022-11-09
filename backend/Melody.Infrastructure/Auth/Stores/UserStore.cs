using Melody.Core.Entities;
using Melody.Core.Interfaces;   
using Microsoft.AspNetCore.Identity;

namespace Melody.Infrastructure.Auth.Stores;


public class UserStore: IUserStore<UserIdentity>
{
    private readonly IUserRepository _userRepository;

    private bool disposedValue;

    public UserStore(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    private long ConvertIdFromString(string userId)
        => long.Parse(userId);

    public async Task<IdentityResult> CreateAsync(UserIdentity user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        user.ThrowIfNull(nameof(user));
        var created = await _userRepository.CreateAsync(user);
        return created ? IdentityResult.Success : IdentityResult.Failed(new IdentityError
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
        return deleted ? IdentityResult.Success : IdentityResult.Failed(new IdentityError
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

    public Task SetNormalizedUserNameAsync(UserIdentity user, string normalizedName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (user == null) throw new ArgumentNullException(nameof(user));
        if (normalizedName == null) throw new ArgumentNullException(nameof(normalizedName));

        user.NormalizedUserName = normalizedName;
        return Task.FromResult<object>(result: null);
    }

    public Task SetUserNameAsync(UserIdentity user, string userName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (user == null) throw new ArgumentNullException(nameof(user));
        if (userName == null) throw new ArgumentNullException(nameof(userName));

        user.UserName = userName;
        return Task.FromResult<object>(result: null);
    }

    public Task<IdentityResult> UpdateAsync(UserIdentity user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        user.ThrowIfNull(nameof(user));
        var updated = await _userRepository.UpdateAsync(user, UserRoles, UserTokens);
        return updated ? IdentityResult.Success : IdentityResult.Failed(new IdentityError
        {
            Code = string.Empty,
            Description = $"UserIdentity '{user.UserName}' could not be deleted."
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
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}   
