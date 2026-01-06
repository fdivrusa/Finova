using Finova.Core.Common;

namespace Finova.Core.Enterprise;

/// <summary>
/// Defines a service for global enterprise/business registration number validation.
/// </summary>
public interface IGlobalEnterpriseValidator
{
    /// <summary>
    /// Validates an enterprise number for a specific country.
    /// </summary>
    /// <param name="number">The enterprise number to validate.</param>
    /// <param name="countryCode">The ISO 3166-1 alpha-2 country code.</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    ValidationResult Validate(string? number, string countryCode);

    /// <summary>
    /// Normalizes an enterprise number for a specific country.
    /// </summary>
    /// <param name="number">The enterprise number to normalize.</param>
    /// <param name="countryCode">The ISO 3166-1 alpha-2 country code.</param>
    /// <returns>The normalized number if valid; otherwise, the original input or null.</returns>
    string? Parse(string? number, string countryCode);
}
