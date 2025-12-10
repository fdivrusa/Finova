using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.Greece.Validators;

public class GreeceIbanValidator : IIbanValidator
{
    public string CountryCode => "GR";
    private const int GreeceIbanLength = 27;

    public ValidationResult Validate(string? iban) => ValidateGreeceIban(iban);

    public static ValidationResult ValidateGreeceIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "IBAN cannot be empty.");
        }
        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != GreeceIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, $"Invalid length. Expected {GreeceIbanLength}, got {normalized.Length}.");
        }

        if (!normalized.StartsWith("GR", StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, "Invalid country code. Expected GR.");
        }


        // Bank Code (Pos 4-6) and Branch Code (Pos 7-10) must be digits
        // Range: 4 to 11
        for (int i = 4; i < 11; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Greece Bank/Branch code must be digits.");
            }
        }

        // Account Number (Pos 11-26) can be Alphanumeric
        // Range: 11 to 27
        for (int i = 11; i < GreeceIbanLength; i++)
        {
            if (!char.IsLetterOrDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Greece Account Number must be alphanumeric.");
            }
        }

        // 4. Checksum Validation (Modulo 97)
        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid checksum.");
    }
}
