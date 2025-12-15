using Finova.Core.Vat;

namespace Finova.Core.Enterprise;

/// <summary>
/// Helper class for normalizing enterprise numbers.
/// </summary>
public static class EnterpriseNumberNormalizer
{
    /// <summary>
    /// Normalizes the enterprise number by sanitizing it and removing the country code prefix if present.
    /// </summary>
    /// <param name="number">The enterprise number to normalize.</param>
    /// <param name="countryCode">The country code to remove (case-insensitive).</param>
    /// <returns>The normalized enterprise number.</returns>
    public static string? Normalize(string? number, string countryCode)
    {
        var sanitized = VatSanitizer.Sanitize(number);

        if (string.IsNullOrWhiteSpace(sanitized))
        {
            return sanitized;
        }

        if (sanitized.StartsWith(countryCode, StringComparison.OrdinalIgnoreCase))
        {
            return sanitized[countryCode.Length..];
        }

        return sanitized;
    }
}
