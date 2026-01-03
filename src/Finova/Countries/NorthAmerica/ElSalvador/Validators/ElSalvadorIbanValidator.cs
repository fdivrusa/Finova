using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.NorthAmerica.ElSalvador.Validators;

/// <summary>
/// Validator for El Salvador IBANs.
/// El Salvador IBAN format: SV + 2 check digits + 4 letters (bank code) + 20 digits (account)
/// Length: 28 characters
/// Example: SV62CENR00000000000000700025
/// </summary>
public class ElSalvadorIbanValidator : IIbanValidator
{
    public string CountryCode => "SV";

    private const int ElSalvadorIbanLength = 28;
    private const string ElSalvadorCountryCode = "SV";

    public ValidationResult Validate(string? iban) => ValidateElSalvadorIban(iban);

    public static ValidationResult ValidateElSalvadorIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != ElSalvadorIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength,
                string.Format(ValidationMessages.InvalidLengthExpectedXGotY, ElSalvadorIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(ElSalvadorCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidCountryCode);
        }

        // Validate BBAN format: 4 letters (bank code) + 20 digits (account)
        string bban = normalized.Substring(4);

        // Bank code (4 letters)
        if (!bban.Substring(0, 4).All(char.IsLetter))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        // Account number (20 digits)
        if (!bban.Substring(4).All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
