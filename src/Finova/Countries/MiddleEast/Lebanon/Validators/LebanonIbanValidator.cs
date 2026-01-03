using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.MiddleEast.Lebanon.Validators;

/// <summary>
/// Validator for Lebanon IBANs.
/// Lebanon IBAN format: LB + 2 check digits + 4 digits (bank code) + 20 alphanumeric (account)
/// Length: 28 characters
/// Example: LB62099900000001001901229114
/// </summary>
public class LebanonIbanValidator : IIbanValidator
{
    public string CountryCode => "LB";

    private const int LebanonIbanLength = 28;
    private const string LebanonCountryCode = "LB";

    public ValidationResult Validate(string? iban) => ValidateLebanonIban(iban);

    public static ValidationResult ValidateLebanonIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != LebanonIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength,
                string.Format(ValidationMessages.InvalidLengthExpectedXGotY, LebanonIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(LebanonCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidCountryCode);
        }

        // Validate BBAN format: 4 digits (bank code) + 20 alphanumeric (account)
        string bban = normalized.Substring(4);

        // Bank code (4 digits)
        if (!bban.Substring(0, 4).All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        // Account number (20 alphanumeric)
        if (!bban.Substring(4).All(char.IsLetterOrDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
