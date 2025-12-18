using System.Text;

namespace Finova.Core.Common;

/// <summary>
/// Helper for sanitizing input strings.
/// </summary>
public static class InputSanitizer
{
    /// <summary>
    /// Removes all non-alphanumeric characters (spaces, dashes, dots, etc.) from the input string and converts to uppercase.
    /// </summary>
    /// <param name="input">The input string.</param>
    /// <returns>The sanitized string, or null if input is null.</returns>
    public static string? Sanitize(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return input;
        }

        // Fast path: check if string is already clean
        bool isClean = true;
        foreach (char c in input)
        {
            if (!char.IsLetterOrDigit(c))
            {
                isClean = false;
                break;
            }
        }

        if (isClean)
        {
            return input.ToUpperInvariant();
        }

        // Slow path: build new string
        var sb = new StringBuilder(input.Length);
        foreach (char c in input)
        {
            if (char.IsLetterOrDigit(c))
            {
                sb.Append(char.ToUpperInvariant(c));
            }
        }

        return sb.ToString();
    }
}
