using System.Diagnostics.CodeAnalysis;

using Finova.Core.Common;
using Finova.Core.Iban;
using Finova.Core.Bic;
using Finova.Core.PaymentCard;
using Finova.Core.PaymentReference;
using Finova.Core.Vat;



namespace Finova.Countries.Europe.Ukraine.Validators;

/// <summary>
/// Validator for Ukraine IBANs.
/// </summary>
public class UkraineIbanValidator : IIbanValidator
{
    /// <summary>
    /// Gets the country code for Ukraine.
    /// </summary>
    public string CountryCode => "UA";

    /// <summary>
    /// Validates the Ukraine IBAN.
    /// </summary>
    /// <param name="iban">The IBAN to validate.</param>
    /// <returns>ValidationResult indicating success or failure.</returns>
    public ValidationResult Validate(string? iban) => ValidateUkraineIban(iban);

    /// <summary>
    /// Static validation method for Ukraine IBANs.
    /// </summary>
    /// <param name="iban">The IBAN to validate.</param>
    /// <returns>True if the IBAN is valid; otherwise, false.</returns>
    public static ValidationResult ValidateUkraineIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != 29)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, 29, normalized.Length));
        }

        if (!normalized.StartsWith("UA", StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, string.Format(ValidationMessages.InvalidCountryCodeExpected, "UA"));
        }

        // Structure Validation:
        // 1. Bank Code (MFO) (Pos 4-10): 6 digits
        for (int i = 4; i < 10; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidUkraineBankCodeFormat);
            }
        }

        // 2. Account Number (Pos 10-29): 19 digits
        for (int i = 10; i < 29; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidUkraineAccountNumberFormat);
            }
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}

