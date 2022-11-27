using Melody.SharedKernel.Interfaces;

namespace Melody.Core.Entities;

public class User : EntityBase<long>
{
    public User(string name, string email, string phoneNumber, long roleId, bool isBanned = false)
    {
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        RoleId = roleId;
        IsBanned = isBanned;
    }

    public string Name { get; }
    public string Email { get; }
    public string PhoneNumber { get; }
    public long RoleId { get; }
    public bool IsBanned { get; }
}