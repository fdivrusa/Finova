using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.Switzerland.Validators;

public class SwitzerlandIbanValidator : IIbanValidator
{
    public string CountryCode => "CH";
    private const int SwitzerlandIbanLength = 21;
    private const string SwitzerlandCountryCode = "CH";

    public ValidationResult Validate(string? iban) => ValidateSwitzerlandIban(iban);

    public static ValidationResult ValidateSwitzerlandIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "IBAN cannot be empty.");
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != SwitzerlandIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, $"Invalid length. Expected {SwitzerlandIbanLength}, got {normalized.Length}.");
        }

        if (!normalized.StartsWith(SwitzerlandCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, "Invalid country code. Expected CH.");
        }

        // Structure Validation:

        // 1. Clearing Number (Pos 4-9): Must be digits
        for (int i = 4; i < 9; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Switzerland Clearing Number must be digits.");
            }
        }

        // 2. Account Number (Pos 9-21): Alphanumeric
        for (int i = 9; i < 21; i++)
        {
            if (!char.IsLetterOrDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Switzerland Account Number must be alphanumeric.");
            }
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid checksum.");
    }
}
