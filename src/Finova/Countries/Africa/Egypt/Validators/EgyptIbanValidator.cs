using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Africa.Egypt.Validators;

/// <summary>
/// Validator for Egypt IBANs.
/// Egypt IBAN format: EG + 2 check digits + 4 digits (bank code) + 4 digits (branch) + 17 digits (account)
/// Length: 29 characters
/// Example: EG380019000500000000263180002
/// </summary>
public class EgyptIbanValidator : IIbanValidator
{
    public string CountryCode => "EG";

    private const int EgyptIbanLength = 29;
    private const string EgyptCountryCode = "EG";

    public ValidationResult Validate(string? iban) => ValidateEgyptIban(iban);

    public static ValidationResult ValidateEgyptIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != EgyptIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength,
                string.Format(ValidationMessages.InvalidLengthExpectedXGotY, EgyptIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(EgyptCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidCountryCode);
        }

        // BBAN (25 digits: 4 bank + 4 branch + 17 account)
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
