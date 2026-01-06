using Finova.Core.Common;

namespace Finova.Core.Identifiers;

/// <summary>
/// Interface for BBAN (Basic Bank Account Number) validators.
/// BBAN is the country-specific bank account format (e.g., Sort Code + Account Number in UK).
/// </summary>
public interface IBbanValidator : IValidator<string>
{
    /// <summary>
    /// Gets the country code (ISO 3166-1 alpha-2) for this validator.
    /// </summary>
    string CountryCode { get; }

    /// <summary>
    /// Parses the BBAN and returns structured details.
    /// Default implementation returns basic details without country-specific parsing.
    /// </summary>
    /// <param name="bban">The BBAN to parse.</param>
    /// <returns>The parsed BBAN details, or null if invalid.</returns>
    BbanDetails? ParseDetails(string? bban)
    {
        var sanitized = Parse(bban);
        if (string.IsNullOrEmpty(sanitized))
        {
            return null;
        }

        return new BbanDetails
        {
            Bban = sanitized,
            CountryCode = CountryCode
        };
    }
}
