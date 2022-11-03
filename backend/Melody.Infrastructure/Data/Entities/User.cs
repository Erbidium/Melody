namespace Melody.Infrastructure.Data.Entities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public int RoleId { get; set; }
    public bool isBanned { get; set; }
    public bool IsDeleted { get; set; }

}
