namespace Melody.Infrastructure.Auth.Models;

public class UserToken
{
    public long UserId { get; set; }
    public IEnumerable<string> Roles { get; set; }
}