using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Gibraltar.Validators;

public class GibraltarIbanValidator : IIbanValidator
{
    public string CountryCode => "GI";
    private const int GibraltarIbanLength = 23;
    private const string GibraltarCountryCode = "GI";

    public ValidationResult Validate(string? iban) => ValidateGibraltarIban(iban);

    public static ValidationResult ValidateGibraltarIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != GibraltarIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, GibraltarIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(GibraltarCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidGibraltarCountryCode);
        }

        // Validate BBAN
        string bban = normalized.Substring(4);
        var bbanResult = GibraltarBbanValidator.Validate(bban);
        if (!bbanResult.IsValid)
        {
            return bbanResult;
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
