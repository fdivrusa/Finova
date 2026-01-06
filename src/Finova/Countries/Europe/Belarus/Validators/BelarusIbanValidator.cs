using System.Diagnostics.CodeAnalysis;

using Finova.Core.Common;
using Finova.Core.Iban;



namespace Finova.Countries.Europe.Belarus.Validators;

/// <summary>
/// Validator for Belarus IBANs.
/// </summary>
public class BelarusIbanValidator : IIbanValidator
{
    /// <summary>
    /// Gets the country code for Belarus.
    /// </summary>
    public string CountryCode => "BY";

    /// <summary>
    /// Validates the Belarus IBAN.
    /// </summary>
    /// <param name="iban">The IBAN to validate.</param>
    /// <returns>ValidationResult indicating success or failure.</returns>
    public ValidationResult Validate(string? iban) => ValidateBelarusIban(iban);

    /// <summary>
    /// Static validation method for Belarus IBANs.
    /// </summary>
    /// <param name="iban">The IBAN to validate.</param>
    /// <returns>True if the IBAN is valid; otherwise, false.</returns>
    public static ValidationResult ValidateBelarusIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.IbanEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != 28)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidIbanLength, 28, normalized.Length));
        }

        if (!normalized.StartsWith("BY", StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, string.Format(ValidationMessages.InvalidCountryCodeExpected, "BY"));
        }

        // Validate BBAN
        string bban = normalized.Substring(4);
        var bbanResult = BelarusBbanValidator.Validate(bban);
        if (!bbanResult.IsValid)
        {
            return bbanResult;
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}

