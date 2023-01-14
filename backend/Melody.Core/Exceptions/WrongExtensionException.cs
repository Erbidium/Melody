namespace Melody.Core.Exceptions;

public class WrongExtensionException : Exception
{
    public WrongExtensionException() : base("Your sound file has wrong extension")
    {
    }
}