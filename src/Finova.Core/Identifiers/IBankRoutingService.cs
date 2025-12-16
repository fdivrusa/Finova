using Finova.Core.Common;

namespace Finova.Core.Identifiers;

/// <summary>
/// Service for validating Bank Routing Numbers (ABA, Transit, BSB, IFSC, etc.) across different countries.
/// </summary>
public interface IBankRoutingService
{
    /// <summary>
    /// Validates a Bank Routing Number for a specific country.
    /// </summary>
    /// <param name="countryCode">The ISO 3166-1 alpha-2 country code (e.g., "US", "CA").</param>
    /// <param name="routingNumber">The Routing Number to validate.</param>
    /// <returns>The validation result.</returns>
    ValidationResult Validate(string countryCode, string? routingNumber);

    /// <summary>
    /// Parses a Bank Routing Number for a specific country.
    /// </summary>
    /// <param name="countryCode">The ISO 3166-1 alpha-2 country code.</param>
    /// <param name="routingNumber">The Routing Number to parse.</param>
    /// <returns>Parsed details or null if invalid/unsupported.</returns>
    BankRoutingDetails? Parse(string countryCode, string? routingNumber);
}
