using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.MiddleEast.Kuwait.Validators;

/// <summary>
/// Validator for Kuwait IBANs.
/// Kuwait IBAN format: KW + 2 check digits + 4 letters (bank code) + 22 alphanumeric (account)
/// Length: 30 characters
/// Example: KW81CBKU0000000000001234560101
/// </summary>
public class KuwaitIbanValidator : IIbanValidator
{
    public string CountryCode => "KW";

    private const int KuwaitIbanLength = 30;
    private const string KuwaitCountryCode = "KW";

    public ValidationResult Validate(string? iban) => ValidateKuwaitIban(iban);

    public static ValidationResult ValidateKuwaitIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != KuwaitIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength,
                string.Format(ValidationMessages.InvalidLengthExpectedXGotY, KuwaitIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(KuwaitCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidCountryCode);
        }

        // Validate BBAN format: 4 letters (bank code) + 22 alphanumeric (account)
        string bban = normalized.Substring(4);

        // Bank code (4 letters)
        if (!bban.Substring(0, 4).All(char.IsLetter))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        // Account number (22 alphanumeric)
        if (!bban.Substring(4).All(char.IsLetterOrDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
