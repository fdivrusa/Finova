using Finova.Core.Common;

namespace Finova.Core.Identifiers;

/// <summary>
/// Service for validating BBAN (Basic Bank Account Number) across different countries.
/// BBAN is the country-specific bank account format (e.g., Sort Code + Account Number in UK).
/// </summary>
public interface IBbanService
{
    /// <summary>
    /// Validates a BBAN for a specific country.
    /// </summary>
    /// <param name="countryCode">The ISO 3166-1 alpha-2 country code (e.g., "BE", "FR", "DE").</param>
    /// <param name="bban">The BBAN to validate.</param>
    /// <returns>The validation result.</returns>
    ValidationResult Validate(string countryCode, string? bban);

    /// <summary>
    /// Parses a BBAN for a specific country and returns its details.
    /// </summary>
    /// <param name="countryCode">The ISO 3166-1 alpha-2 country code.</param>
    /// <param name="bban">The BBAN to parse.</param>
    /// <returns>Parsed details or null if invalid/unsupported.</returns>
    BbanDetails? Parse(string countryCode, string? bban);
}
