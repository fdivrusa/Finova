using Finova.Core.Common;

namespace Finova.Core.Enterprise;

/// <summary>
/// Interface for validating Enterprise/Business Registration Numbers (e.g., SIRET, Firmenbuchnummer, KBO).
/// </summary>
public interface IEnterpriseValidator : IValidator<string>
{
    /// <summary>
    /// ISO country code (ex: "BE", "AT", "FR")
    /// </summary>
    string CountryCode { get; }

    /// <summary>
    /// Validates an enterprise number using the country code.
    /// </summary>
    /// <param name="enterpriseNumber">The enterprise number to validate.</param>
    /// <param name="countryCode">The ISO country code (ex: "BE", "AT", "FR").</param>
    /// <returns>Validation result.</returns>
    ValidationResult Validate(string? enterpriseNumber, string countryCode);

    /// <summary>
    /// Validates an enterprise number using the enterprise number type.
    /// </summary>
    /// <param name="enterpriseNumber">The enterprise number to validate.</param>
    /// <param name="enterpriseNumberType">The enterprise number type (e.g., SIRET, Firmenbuchnummer, KBO).</param>
    /// <returns>Validation result.</returns>
    ValidationResult Validate(string? enterpriseNumber, EnterpriseNumberType enterpriseNumberType);
}
