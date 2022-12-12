namespace Melody.Core.Exceptions;

public class UploadLimitException : Exception
{
    public UploadLimitException() : base("You have reached your upload limit 1 Gb")
    {
    }
}