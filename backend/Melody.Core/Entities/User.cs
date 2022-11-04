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

    public long Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public long RoleId { get; set; }
    public bool IsBanned { get; set; }

}
