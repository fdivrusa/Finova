using Finova.Core.Common;

namespace Finova.Core.Identifiers;

/// <summary>
/// Service for validating National IDs (SSN, SIN, CPF, etc.) across different countries.
/// </summary>
public interface INationalIdService
{
    /// <summary>
    /// Validates a National ID for a specific country.
    /// </summary>
    /// <param name="countryCode">The ISO 3166-1 alpha-2 country code (e.g., "US", "CA").</param>
    /// <param name="nationalId">The National ID to validate.</param>
    /// <returns>The validation result.</returns>
    ValidationResult Validate(string countryCode, string? nationalId);
}
