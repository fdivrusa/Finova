using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.MiddleEast.Israel.Validators;

/// <summary>
/// Validator for Israel IBANs.
/// Israel IBAN format: IL + 2 check digits + 3 digits (bank code) + 3 digits (branch code) + 13 digits (account)
/// Length: 23 characters
/// Example: IL620108000000099999999
/// </summary>
public class IsraelIbanValidator : IIbanValidator
{
    public string CountryCode => "IL";

    private const int IsraelIbanLength = 23;
    private const string IsraelCountryCode = "IL";

    public ValidationResult Validate(string? iban) => ValidateIsraelIban(iban);

    public static ValidationResult ValidateIsraelIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != IsraelIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength,
                string.Format(ValidationMessages.InvalidLengthExpectedXGotY, IsraelIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(IsraelCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidCountryCode);
        }

        // BBAN (19 digits)
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
