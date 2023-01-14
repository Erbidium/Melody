namespace Melody.Infrastructure.Data.DbEntites;

public class RefreshTokenDb
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string RefreshToken { get; set; }
}