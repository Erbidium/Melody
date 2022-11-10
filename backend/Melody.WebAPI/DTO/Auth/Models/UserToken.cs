namespace Melody.Infrastructure.Auth.Models;

public class UserToken
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public IEnumerable<string> Roles { get; set; }
}
