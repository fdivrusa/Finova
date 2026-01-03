using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.MiddleEast.Qatar.Validators;

/// <summary>
/// Validator for Qatar IBANs.
/// Qatar IBAN format: QA + 2 check digits + 4 letters (bank code) + 21 alphanumeric (account)
/// Length: 29 characters
/// Example: QA58DOHB00001234567890ABCDEFG
/// </summary>
public class QatarIbanValidator : IIbanValidator
{
    public string CountryCode => "QA";

    private const int QatarIbanLength = 29;
    private const string QatarCountryCode = "QA";

    public ValidationResult Validate(string? iban) => ValidateQatarIban(iban);

    public static ValidationResult ValidateQatarIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != QatarIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength,
                string.Format(ValidationMessages.InvalidLengthExpectedXGotY, QatarIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(QatarCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidCountryCode);
        }

        // Validate BBAN format: 4 letters (bank code) + 21 alphanumeric (account)
        string bban = normalized.Substring(4);

        // Bank code (4 letters)
        if (!bban.Substring(0, 4).All(char.IsLetter))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        // Account number (21 alphanumeric)
        if (!bban.Substring(4).All(char.IsLetterOrDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
