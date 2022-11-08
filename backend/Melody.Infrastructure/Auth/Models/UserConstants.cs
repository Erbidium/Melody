namespace Melody.Infrastructure.Auth.Models;

public class UserConstants
{
    public static List<UserModel> Users = new List<UserModel>()
    {
        new UserModel() { UserName = "User_admin", EmailAdress = "User_admin@email.com", Password = "MyPass_w0rd",
            GivenName = "Jason", Surname = "Bryan", Role = "Administrator" },
        new UserModel() { UserName = "User_random", EmailAdress = "User_random@email.com", Password = "MyPass_w0rd",
            GivenName = "Max", Surname = "Johnson", Role = "Seller" },
    };
}
