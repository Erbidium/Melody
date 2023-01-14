namespace Melody.Core.Exceptions;

public class RegistrationErrorException : Exception
{
    public RegistrationErrorException(IReadOnlyList<string> errors)
    {
        Errors = errors;
    }
    
    public IReadOnlyList<string> Errors { get; }
}