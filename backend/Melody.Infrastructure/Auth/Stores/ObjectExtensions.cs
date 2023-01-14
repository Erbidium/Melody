namespace Melody.Infrastructure.Auth.Stores;

internal static class ObjectExtensions
{
    internal static void ThrowIfNull<T>(this T @object, string paramName)
    {
        if (@object is null) throw new ArgumentNullException(paramName, $"Parameter {paramName} cannot be null.");
    }
}