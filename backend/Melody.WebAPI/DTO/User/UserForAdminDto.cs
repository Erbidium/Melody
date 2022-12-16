namespace Melody.WebAPI.DTO.User;

public class UserForAdminDto
{
    public long Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsBanned { get; set; }
}