
using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;



namespace Finova.Countries.Europe.Azerbaijan.Validators;

/// <summary>
/// Validator for Azerbaijan IBANs.
/// </summary>
public class AzerbaijanIbanValidator : IIbanValidator
{
    /// <summary>
    /// Gets the country code for Azerbaijan.
    /// </summary>
    public string CountryCode => "AZ";

    /// <summary>
    /// Validates the Azerbaijan IBAN.
    /// </summary>
    /// <param name="iban">The IBAN to validate.</param>
    /// <returns>ValidationResult indicating success or failure.</returns>
    public ValidationResult Validate(string? iban) => ValidateAzerbaijanIban(iban);

    /// <summary>
    /// Static validation method for Azerbaijan IBANs.
    /// </summary>
    /// <param name="iban">The IBAN to validate.</param>
    /// <returns>True if the IBAN is valid; otherwise, false.</returns>
    public static ValidationResult ValidateAzerbaijanIban([NotNullWhen(true)] string? iban)
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

        if (!normalized.StartsWith("AZ", StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, string.Format(ValidationMessages.InvalidCountryCodeExpected, "AZ"));
        }

        // Validate BBAN
        string bban = normalized.Substring(4);
        var bbanResult = AzerbaijanBbanValidator.Validate(bban);
        if (!bbanResult.IsValid)
        {
            return bbanResult;
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}

