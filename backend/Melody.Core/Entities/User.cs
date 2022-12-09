using Melody.SharedKernel.Interfaces;

namespace Melody.Core.Entities;

public class User : EntityBase<long>
{
    public User(string username, string email, string phoneNumber, bool isBanned = false)
    {
        UserName = username;
        Email = email;
        PhoneNumber = phoneNumber;
        IsBanned = isBanned;
    }

    public string UserName { get; }
    public string Email { get; }
    public string PhoneNumber { get; }
    public bool IsBanned { get; }
    public IList<string>? Roles { get; set; }
}