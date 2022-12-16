using Melody.Core.Entities;

namespace Melody.Core.Interfaces;

public interface IUserRepository
{
    Task<IReadOnlyCollection<User>> GetUsersWithoutAdministratorRole();
}