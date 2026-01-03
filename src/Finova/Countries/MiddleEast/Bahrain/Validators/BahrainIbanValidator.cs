using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.MiddleEast.Bahrain.Validators;

/// <summary>
/// Validator for Bahrain IBANs.
/// Bahrain IBAN format: BH + 2 check digits + 4 letters (bank code) + 14 alphanumeric (account)
/// Length: 22 characters
/// Example: BH67BMAG00001299123456
/// </summary>
public class BahrainIbanValidator : IIbanValidator
{
    public string CountryCode => "BH";

    private const int BahrainIbanLength = 22;
    private const string BahrainCountryCode = "BH";

    public ValidationResult Validate(string? iban) => ValidateBahrainIban(iban);

    public static ValidationResult ValidateBahrainIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != BahrainIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength,
                string.Format(ValidationMessages.InvalidLengthExpectedXGotY, BahrainIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(BahrainCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidCountryCode);
        }

        // Validate BBAN format: 4 letters (bank code) + 14 alphanumeric (account)
        string bban = normalized.Substring(4);
        if (!bban.Substring(0, 4).All(char.IsLetter))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        if (!bban.Substring(4).All(char.IsLetterOrDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
