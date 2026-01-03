using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.SouthAmerica.Brazil.Validators;

/// <summary>
/// Validator for Brazil IBANs.
/// Brazil IBAN format: BR + 2 check digits + 8 digits (bank code) + 5 digits (branch) + 10 digits (account) + 1 letter (account type) + 1 letter (owner)
/// Length: 29 characters
/// Example: BR1800360305000010009795493C1
/// </summary>
public class BrazilIbanValidator : IIbanValidator
{
    public string CountryCode => "BR";

    private const int BrazilIbanLength = 29;
    private const string BrazilCountryCode = "BR";

    public ValidationResult Validate(string? iban) => ValidateBrazilIban(iban);

    public static ValidationResult ValidateBrazilIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != BrazilIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength,
                string.Format(ValidationMessages.InvalidLengthExpectedXGotY, BrazilIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(BrazilCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidCountryCode);
        }

        // Validate BBAN format: 8 digits (bank code) + 5 digits (branch) + 10 digits (account) + 1 letter (type) + 1 letter (owner)
        string bban = normalized.Substring(4);

        // Bank code (8 digits)
        if (!bban.Substring(0, 8).All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        // Branch code (5 digits)
        if (!bban.Substring(8, 5).All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        // Account number (10 digits)
        if (!bban.Substring(13, 10).All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        // Account type (1 letter) and Owner indicator (1 letter)
        string accountType = bban.Substring(23, 1);
        string ownerIndicator = bban.Substring(24, 1);
        if (!char.IsLetter(accountType[0]) || !char.IsLetterOrDigit(ownerIndicator[0]))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
