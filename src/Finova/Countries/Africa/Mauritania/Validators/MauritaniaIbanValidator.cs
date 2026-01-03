using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Africa.Mauritania.Validators;

/// <summary>
/// Validator for Mauritania IBANs.
/// Mauritania IBAN format: MR + 2 check digits + 5 digits (bank code) + 5 digits (branch) + 11 digits (account) + 2 digits (key)
/// Length: 27 characters
/// Example: MR1300020001010000123456753
/// </summary>
public class MauritaniaIbanValidator : IIbanValidator
{
    public string CountryCode => "MR";

    private const int MauritaniaIbanLength = 27;
    private const string MauritaniaCountryCode = "MR";

    public ValidationResult Validate(string? iban) => ValidateMauritaniaIban(iban);

    public static ValidationResult ValidateMauritaniaIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != MauritaniaIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength,
                string.Format(ValidationMessages.InvalidLengthExpectedXGotY, MauritaniaIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(MauritaniaCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidCountryCode);
        }

        // BBAN (23 digits)
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
