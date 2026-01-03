using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.MiddleEast.SaudiArabia.Validators;

/// <summary>
/// Validator for Saudi Arabia IBANs.
/// Saudi Arabia IBAN format: SA + 2 check digits + 2 digits (bank code) + 18 alphanumeric (account)
/// Length: 24 characters
/// Example: SA0380000000608010167519
/// </summary>
public class SaudiArabiaIbanValidator : IIbanValidator
{
    public string CountryCode => "SA";

    private const int SaudiArabiaIbanLength = 24;
    private const string SaudiArabiaCountryCode = "SA";

    public ValidationResult Validate(string? iban) => ValidateSaudiArabiaIban(iban);

    public static ValidationResult ValidateSaudiArabiaIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != SaudiArabiaIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength,
                string.Format(ValidationMessages.InvalidLengthExpectedXGotY, SaudiArabiaIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(SaudiArabiaCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidCountryCode);
        }

        // Validate BBAN format: 2 digits (bank code) + 18 alphanumeric (account)
        string bban = normalized.Substring(4);

        // Bank code (2 digits)
        if (!bban.Substring(0, 2).All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        // Account number (18 alphanumeric)
        if (!bban.Substring(2).All(char.IsLetterOrDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
