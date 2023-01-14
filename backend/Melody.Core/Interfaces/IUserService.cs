using LanguageExt.Common;
using Melody.Core.Entities;

namespace Melody.Core.Interfaces;

public interface IUserService
{
    Task<string> GetUserEmail(long userId);
    Task<bool> UsernameIsUsed(string username);
    Task<bool> EmailIsUsed(string email);
    Task<Result<bool>> Register(User userRegister, string password);
    Task<UserWithRoles?> GetUserById(long id);
}