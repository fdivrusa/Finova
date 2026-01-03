using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Asia.TimorLeste.Validators;

/// <summary>
/// Validator for Timor-Leste (East Timor) IBANs.
/// Timor-Leste IBAN format: TL + 2 check digits + 3 digits (bank code) + 14 digits (account) + 2 digits (check)
/// Length: 23 characters
/// Example: TL380080012345678910157
/// </summary>
public class TimorLesteIbanValidator : IIbanValidator
{
    public string CountryCode => "TL";

    private const int TimorLesteIbanLength = 23;
    private const string TimorLesteCountryCode = "TL";

    public ValidationResult Validate(string? iban) => ValidateTimorLesteIban(iban);

    public static ValidationResult ValidateTimorLesteIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != TimorLesteIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength,
                string.Format(ValidationMessages.InvalidLengthExpectedXGotY, TimorLesteIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(TimorLesteCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidCountryCode);
        }

        // BBAN (19 digits: 3 bank + 14 account + 2 check)
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
