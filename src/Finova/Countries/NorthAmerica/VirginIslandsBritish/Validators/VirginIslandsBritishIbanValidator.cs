using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.NorthAmerica.VirginIslandsBritish.Validators;

/// <summary>
/// Validator for British Virgin Islands IBANs.
/// British Virgin Islands IBAN format: VG + 2 check digits + 4 letters (bank code) + 16 digits (account)
/// Length: 24 characters
/// Example: VG96VPVG0000012345678901
/// </summary>
public class VirginIslandsBritishIbanValidator : IIbanValidator
{
    public string CountryCode => "VG";

    private const int VirginIslandsBritishIbanLength = 24;
    private const string VirginIslandsBritishCountryCode = "VG";

    public ValidationResult Validate(string? iban) => ValidateVirginIslandsBritishIban(iban);

    public static ValidationResult ValidateVirginIslandsBritishIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != VirginIslandsBritishIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength,
                string.Format(ValidationMessages.InvalidLengthExpectedXGotY, VirginIslandsBritishIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(VirginIslandsBritishCountryCode, StringComparison.OrdinalIgnoreCase))
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
