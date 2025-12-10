using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.Malta.Validators;

public class MaltaIbanValidator : IIbanValidator
{
    public string CountryCode => "MT";
    private const int MaltaIbanLength = 31;
    private const string MaltaCountryCode = "MT";

    public ValidationResult Validate(string? iban) => ValidateMaltaIban(iban);

    public static ValidationResult ValidateMaltaIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "IBAN cannot be empty.");
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != MaltaIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, $"Invalid length. Expected {MaltaIbanLength}, got {normalized.Length}.");
        }

        if (!normalized.StartsWith(MaltaCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, "Invalid country code. Expected MT.");
        }

        // Structure Validation:

        // 1. Bank BIC (Pos 4-8): Must be letters
        for (int i = 4; i < 8; i++)
        {
            if (!char.IsLetter(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Malta Bank BIC must be letters.");
            }
        }

        // 2. Sort Code (Pos 8-13): Must be digits
        for (int i = 8; i < 13; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Malta Sort Code must be digits.");
            }
        }

        // 3. Account Number (Pos 13-31): Alphanumeric
        for (int i = 13; i < 31; i++)
        {
            if (!char.IsLetterOrDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Malta Account Number must be alphanumeric.");
            }
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid checksum.");
    }
}
