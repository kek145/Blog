using System.Text.RegularExpressions;

namespace BlogAPI.BL.Validators;

public static class BaseValidator<T>
{
    public static bool IsEmailValid(string email)
    {
        var pattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";

        var regex = new Regex(pattern, RegexOptions.IgnoreCase);
        return regex.IsMatch(email);
    }
    public static bool CheckWhitespace(T data)
    {
        if (data is string value)
            return !value.Contains(" ");

        return false;
    }

    public static bool ValidateFieldsNotEmpty(T data)
    {
        if (data is string value)
            return !string.IsNullOrEmpty(value);

        return false;
    }
}