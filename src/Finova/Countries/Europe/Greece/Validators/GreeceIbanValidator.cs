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
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }
        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != GreeceIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, GreeceIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith("GR", StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidGreeceCountryCode);
        }

        // Validate BBAN
        string bban = normalized.Substring(4);
        var bbanResult = GreeceBbanValidator.Validate(bban);
        if (!bbanResult.IsValid)
        {
            return bbanResult;
        }

        // 4. Checksum Validation (Modulo 97)
        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
