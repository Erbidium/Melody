namespace Melody.Core.Exceptions;

public class WrongIdException : Exception
{
    public WrongIdException()
    { }

    public WrongIdException(string message)
        : base(message)
    { }

    public WrongIdException(string message, Exception inner)
        : base(message, inner)
    { }
}
