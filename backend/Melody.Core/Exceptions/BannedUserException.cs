namespace Melody.Core.Exceptions;

public class BannedUserException : Exception
{
    public BannedUserException() : base("Your account is banned")
    {
    }
}
