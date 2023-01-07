using Ardalis.GuardClauses;
using Melody.SharedKernel.Interfaces;

namespace Melody.Core.Entities;

public class User : EntityBase<long>
{
    public User(string userName, string email, string phoneNumber, bool isBanned = false)
    {
        UserName = Guard.Against.NullOrWhiteSpace(userName, nameof(UserName));
        Email = Guard.Against.NullOrWhiteSpace(email, nameof(Email));
        PhoneNumber = Guard.Against.NullOrWhiteSpace(phoneNumber, nameof(PhoneNumber));
        IsBanned = isBanned;
    }

    public string UserName { get; }
    public string Email { get; }
    public string PhoneNumber { get; }
    public bool IsBanned { get; }
}