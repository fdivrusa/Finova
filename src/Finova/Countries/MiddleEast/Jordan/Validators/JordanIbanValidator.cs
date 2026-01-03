using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.MiddleEast.Jordan.Validators;

/// <summary>
/// Validator for Jordan IBANs.
/// Jordan IBAN format: JO + 2 check digits + 4 letters (bank code) + 4 digits (branch) + 18 alphanumeric (account)
/// Length: 30 characters
/// Example: JO94CBJO0010000000000131000302
/// </summary>
public class JordanIbanValidator : IIbanValidator
{
    public string CountryCode => "JO";

    private const int JordanIbanLength = 30;
    private const string JordanCountryCode = "JO";

    public ValidationResult Validate(string? iban) => ValidateJordanIban(iban);

    public static ValidationResult ValidateJordanIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != JordanIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength,
                string.Format(ValidationMessages.InvalidLengthExpectedXGotY, JordanIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(JordanCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidCountryCode);
        }

        // Validate BBAN format: 4 letters (bank code) + 4 digits (branch) + 18 alphanumeric (account)
        string bban = normalized.Substring(4);

        // Bank code (4 letters)
        if (!bban.Substring(0, 4).All(char.IsLetter))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        // Branch code (4 digits)
        if (!bban.Substring(4, 4).All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        // Account number (18 alphanumeric)
        if (!bban.Substring(8).All(char.IsLetterOrDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
