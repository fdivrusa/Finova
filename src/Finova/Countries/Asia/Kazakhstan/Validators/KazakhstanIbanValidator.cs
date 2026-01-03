using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Asia.Kazakhstan.Validators;

/// <summary>
/// Validator for Kazakhstan IBANs.
/// Kazakhstan IBAN format: KZ + 2 check digits + 3 digits (bank code) + 13 alphanumeric (account)
/// Length: 20 characters
/// Example: KZ86125KZT5004100100
/// </summary>
public class KazakhstanIbanValidator : IIbanValidator
{
    public string CountryCode => "KZ";

    private const int KazakhstanIbanLength = 20;
    private const string KazakhstanCountryCode = "KZ";

    public ValidationResult Validate(string? iban) => ValidateKazakhstanIban(iban);

    public static ValidationResult ValidateKazakhstanIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != KazakhstanIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength,
                string.Format(ValidationMessages.InvalidLengthExpectedXGotY, KazakhstanIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(KazakhstanCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidCountryCode);
        }

        // Validate BBAN format: 3 digits (bank code) + 13 alphanumeric (account)
        string bban = normalized.Substring(4);

        // Bank code (3 digits)
        if (!bban.Substring(0, 3).All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        // Account number (13 alphanumeric)
        if (!bban.Substring(3).All(char.IsLetterOrDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
