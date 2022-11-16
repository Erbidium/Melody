namespace Melody.WebAPI.DTO.User;

public class NewUserDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public long RoleId { get; set; }
}