namespace Melody.WebAPI.DTO.User;

public class UserDto
{
    public long Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public IList<string> Roles { get; set; }
}