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
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != MaltaIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, MaltaIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(MaltaCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidMaltaCountryCode);
        }

        // Validate BBAN
        string bban = normalized.Substring(4);
        var bbanResult = MaltaBbanValidator.Validate(bban);
        if (!bbanResult.IsValid)
        {
            return bbanResult;
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
