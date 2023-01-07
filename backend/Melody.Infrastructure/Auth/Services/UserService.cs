using LanguageExt.Common;
using Melody.Core.Entities;
using Melody.Core.Exceptions;
using Melody.Core.Interfaces;
using Melody.Infrastructure.Data.DbEntites;
using Microsoft.AspNetCore.Identity;

namespace Melody.Infrastructure.Auth.Services;

public class UserService : IUserService
{
    private readonly UserManager<UserIdentity> _userManager;

    public UserService(UserManager<UserIdentity> userManager)
    {
        _userManager = userManager;
    }
    public async Task<string> GetUserEmail(long userId)
    {
        return (await _userManager.FindByIdAsync(userId.ToString())).Email;
    }

    public async Task<bool> UsernameIsUsed(string username)
    {
        return await _userManager.FindByNameAsync(username) != null;
    }

    public async Task<bool> EmailIsUsed(string email)
    {
        return await _userManager.FindByEmailAsync(email) != null;
    }

    public async Task<Result<bool>> Register(User userRegister, string password)
    {
        var user = new UserIdentity
        {
            UserName = userRegister.UserName,
            Email = userRegister.Email,
            PhoneNumber = userRegister.PhoneNumber
        };

        var createUserResult = await _userManager.CreateAsync(user, password);
        if (!createUserResult.Succeeded)
        {
            return ReturnIdentityErrors(createUserResult);
        }
        var addRoleResult = await _userManager.AddToRoleAsync(user, "User");

        return !addRoleResult.Succeeded ? ReturnIdentityErrors(addRoleResult) : true;

        Result<bool> ReturnIdentityErrors(IdentityResult identityResult)
        {
            var errors = identityResult.Errors.Select(e => e.Description).ToList();
            return new Result<bool>(new RegistrationErrorException(errors));
        }
    }

    public async Task<UserWithRoles?> GetUserById(long id)
    {
        var userIdentity = await _userManager.FindByIdAsync(id.ToString());
        var roles = await _userManager.GetRolesAsync(userIdentity);
        return userIdentity == null || roles == null
            ? null
            : new UserWithRoles(
                roles,
                userIdentity.UserName,
                userIdentity.Email,
                userIdentity.PhoneNumber,
                userIdentity.IsBanned
            );
    }
}