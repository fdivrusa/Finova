using System.Diagnostics.CodeAnalysis;

using Finova.Core.Common;
using Finova.Core.Iban;
using Finova.Core.Bic;
using Finova.Core.PaymentCard;
using Finova.Core.PaymentReference;
using Finova.Core.Vat;



namespace Finova.Countries.Europe.Turkey.Validators;

/// <summary>
/// Validator for Turkey IBANs.
/// </summary>
public class TurkeyIbanValidator : IIbanValidator
{
    /// <summary>
    /// Gets the country code for Turkey.
    /// </summary>
    public string CountryCode => "TR";

    /// <summary>
    /// Validates the Turkey IBAN.
    /// </summary>
    /// <param name="iban">The IBAN to validate.</param>
    /// <returns>ValidationResult indicating success or failure.</returns>
    public ValidationResult Validate(string? iban) => ValidateTurkeyIban(iban);

    /// <summary>
    /// Static validation method for Turkey IBANs.
    /// </summary>
    /// <param name="iban">The IBAN to validate.</param>
    /// <returns>True if the IBAN is valid; otherwise, false.</returns>
    public static ValidationResult ValidateTurkeyIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != 26)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidIbanLength, 26, normalized.Length));
        }

        if (!normalized.StartsWith("TR", StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidCountryCode);
        }

        // Structure Validation:
        // 1. Bank Code (Pos 4-9): 5 digits
        for (int i = 4; i < 9; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidTurkeyBankCode);
            }
        }

        // 2. Reserve (Pos 9): 1 alphanumeric character
        if (!char.IsLetterOrDigit(normalized[9]))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidTurkeyReserveChar);
        }

        // 3. Account Number (Pos 10-26): 16 alphanumeric characters
        for (int i = 10; i < 26; i++)
        {
            if (!char.IsLetterOrDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidTurkeyAccountNumber);
            }
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}

