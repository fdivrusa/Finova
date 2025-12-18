using Finova.Core.Common;

namespace Finova.Core.Identifiers;

/// <summary>
/// Service for validating Tax IDs (TIN, EIN, VAT, etc.) across different countries.
/// </summary>
public interface ITaxIdService
{
    /// <summary>
    /// Validates a Tax ID for a specific country.
    /// </summary>
    /// <param name="countryCode">The ISO 3166-1 alpha-2 country code (e.g., "US", "CA").</param>
    /// <param name="taxId">The Tax ID to validate.</param>
    /// <returns>The validation result.</returns>
    ValidationResult Validate(string countryCode, string? taxId);
}
