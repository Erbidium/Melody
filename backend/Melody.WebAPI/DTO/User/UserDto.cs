using Melody.Infrastructure.Data.DbEntites;

namespace Melody.WebAPI.DTO.User;

public class UserDto
{
    public UserDto(UserIdentity userIdentity)
    {
        Id = userIdentity.Id;
        UserName = userIdentity.UserName;
        Email = userIdentity.Email;
        PhoneNumber = userIdentity.PhoneNumber;
    }

    public long Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public IList<string> Roles { get; set; }
}