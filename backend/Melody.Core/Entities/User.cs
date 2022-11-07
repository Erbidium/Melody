using Melody.Core.Exceptions;

namespace Melody.Core.Entities;

public class User
{
    public User(string name, string email, string phoneNumber, long roleId, bool isBanned, long id = -1)
    {
        Id = id;
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        RoleId = roleId;
        IsBanned = isBanned;
    }

    private long _id;
    public long Id
    {
        get => _id < 0 ? throw new WrongIdException() : _id;
        set => _id = value;
    }
    public string Name { get; }
    public string Email { get; }
    public string PhoneNumber { get; }
    public long RoleId { get; }
    public bool IsBanned { get; }

}
