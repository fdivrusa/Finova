using Finova.Core.Common;

namespace Finova.Core.Bic;

public interface IBicValidator : IValidator<BicDetails>
{
    /// <summary>
    /// Validates the structure of a BIC/SWIFT code.
    /// </summary>
    /// <summary>
    /// Validates if the BIC's country code matches the provided IBAN country code.
    /// </summary>
    ValidationResult ValidateConsistencyWithIban(string? bic, string? ibanCountryCode);

    /// <summary>
    /// Validates if the BIC is compatible with the provided IBAN.
    /// </summary>
    ValidationResult ValidateCompatibilityWithIban(string? bic, string? iban);
}
