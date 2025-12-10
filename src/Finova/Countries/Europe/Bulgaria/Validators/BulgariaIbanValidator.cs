using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.Bulgaria.Validators;

public class BulgariaIbanValidator : IIbanValidator
{
    public string CountryCode => "BG";
    private const int BulgariaIbanLength = 22;
    private const string BulgariaCountryCode = "BG";

    public ValidationResult Validate(string? iban) => ValidateBulgariaIban(iban);

    public static ValidationResult ValidateBulgariaIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "IBAN cannot be empty.");
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != BulgariaIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, $"Invalid length. Expected {BulgariaIbanLength}, got {normalized.Length}.");
        }

        if (!normalized.StartsWith(BulgariaCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, "Invalid country code. Expected BG.");
        }

        // Structure check: Alphanumeric (Bank ID is letters, Account can be alphanumeric)
        for (int i = 4; i < BulgariaIbanLength; i++)
        {
            if (!char.IsLetterOrDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Bulgaria IBAN must contain only alphanumeric characters after the country code.");
            }
        }

        // Specific check: Bank Code (Pos 4-8) must be letters
        for (int i = 4; i < 8; i++)
        {
            if (!char.IsLetter(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Bulgaria Bank Code must be letters.");
            }
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid checksum.");
    }
}
