using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Asia.Pakistan.Validators;

/// <summary>
/// Validator for Pakistan IBANs.
/// Pakistan IBAN format: PK + 2 check digits + 4 letters (bank code) + 16 digits (account)
/// Length: 24 characters
/// Example: PK36SCBL0000001123456702
/// </summary>
public class PakistanIbanValidator : IIbanValidator
{
    public string CountryCode => "PK";

    private const int PakistanIbanLength = 24;
    private const string PakistanCountryCode = "PK";

    public ValidationResult Validate(string? iban) => ValidatePakistanIban(iban);

    public static ValidationResult ValidatePakistanIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != PakistanIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength,
                string.Format(ValidationMessages.InvalidLengthExpectedXGotY, PakistanIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(PakistanCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidCountryCode);
        }

        // Validate BBAN format: 4 letters (bank code) + 16 digits (account)
        string bban = normalized.Substring(4);

        // Bank code (4 letters)
        if (!bban.Substring(0, 4).All(char.IsLetter))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        // Account number (16 digits)
        if (!bban.Substring(4).All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
