using System.Text.RegularExpressions;

namespace Finova.Core.Vat;

public static partial class VatSanitizer
{
    [GeneratedRegex(@"[^a-zA-Z0-9]")]
    private static partial Regex NonAlphaNumericRegex();

    /// <summary>
    /// Removes all non-alphanumeric characters (spaces, dashes, dots, etc.) from the input string.
    /// Returns null if the input is null.
    /// </summary>
    public static string? Sanitize(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return input;
        }

        return NonAlphaNumericRegex().Replace(input, string.Empty).ToUpperInvariant();
    }
}
