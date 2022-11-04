namespace Melody.Core.Entities;

public class User
{
    public User(long id, string name, string email, string phoneNumber, long roleId, bool isBanned)
    {
        Id = id;
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        RoleId = roleId;
        IsBanned = isBanned;
    }

    public long Id { get; }
    public string Name { get; }
    public string Email { get; }
    public string PhoneNumber { get; }
    public long RoleId { get; }
    public bool IsBanned { get; }

}
