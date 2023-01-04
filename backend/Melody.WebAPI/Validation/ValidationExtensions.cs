using System.Text.RegularExpressions;

namespace Melody.WebAPI.Validation;

public static class ValidationExtensions
{
    private static readonly IReadOnlyDictionary<string, string> RegularExpressions = new Dictionary<string, string>
    {
        { "Password", @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$"},
        { "Email", @"^([a-zA-z0-9]+([._\-][a-zA-z0-9]+)?)+@([a-zA-z0-9]+([.\-][a-zA-Z0-9]+)?)+\.[a-zA-Z]{2,4}$" },
        { "PhoneNumber", @"^(\+38\d{10})$"}
    };
    
    public static bool IsValidPassword(this string value)
    {
        var pattern = RegularExpressions["Password"];
        return Regex.IsMatch(value, pattern);
    }

    public static bool IsValidEmail(this string value)
    {
        var pattern = RegularExpressions["Email"];
        return Regex.IsMatch(value, pattern);
    }

    public static bool IsValidPhoneNumber(this string value)
    {
        var pattern = RegularExpressions["PhoneNumber"];
        return Regex.IsMatch(value, pattern);
    }
}