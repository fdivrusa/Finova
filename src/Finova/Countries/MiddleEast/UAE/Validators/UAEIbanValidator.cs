using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.MiddleEast.UAE.Validators;

/// <summary>
/// Validator for United Arab Emirates IBANs.
/// UAE IBAN format: AE + 2 check digits + 3 digits (bank code) + 16 digits (account)
/// Length: 23 characters
/// Example: AE070331234567890123456
/// </summary>
public class UAEIbanValidator : IIbanValidator
{
    public string CountryCode => "AE";

    private const int UAEIbanLength = 23;
    private const string UAECountryCode = "AE";

    public ValidationResult Validate(string? iban) => ValidateUAEIban(iban);

    public static ValidationResult ValidateUAEIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != UAEIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength,
                string.Format(ValidationMessages.InvalidLengthExpectedXGotY, UAEIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(UAECountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidCountryCode);
        }

        // BBAN (19 digits: 3 bank code + 16 account)
        string bban = normalized.Substring(4);
        if (!bban.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
