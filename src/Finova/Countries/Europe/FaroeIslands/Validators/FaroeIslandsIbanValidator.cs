using System.Diagnostics.CodeAnalysis;

using Finova.Core.Common;
using Finova.Core.Iban;



namespace Finova.Countries.Europe.FaroeIslands.Validators;

/// <summary>
/// Validator for Faroe Islands IBANs.
/// </summary>
public class FaroeIslandsIbanValidator : IIbanValidator
{
    /// <summary>
    /// Gets the country code for Faroe Islands.
    /// </summary>
    public string CountryCode => "FO";

    /// <summary>
    /// Validates the Faroe Islands IBAN.
    /// </summary>
    /// <param name="iban">The IBAN to validate.</param>
    /// <returns>ValidationResult indicating success or failure.</returns>
    public ValidationResult Validate(string? iban) => ValidateFaroeIslandsIban(iban);

    /// <summary>
    /// Static validation method for Faroe Islands IBANs.
    /// </summary>
    /// <param name="iban">The IBAN to validate.</param>
    /// <returns>True if the IBAN is valid; otherwise, false.</returns>
    public static ValidationResult ValidateFaroeIslandsIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.IbanEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != 18)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidIbanLength, 18, normalized.Length));
        }

        if (!normalized.StartsWith("FO", StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, string.Format(ValidationMessages.InvalidCountryCodeExpected, "FO"));
        }

        // FO + 16 digits
        // Indices 4-17 must be digits
        for (int i = 4; i < 18; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, string.Format(ValidationMessages.InvalidIbanDigitsOnly, "Faroe Islands"));
            }
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}

