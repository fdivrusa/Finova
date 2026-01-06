using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Estonia.Validators;

public class EstoniaIbanValidator : IIbanValidator
{
    public string CountryCode => "EE";
    private const int EstoniaIbanLength = 20;
    private const string EstoniaCountryCode = "EE";

    public ValidationResult Validate(string? iban) => ValidateEstoniaIban(iban);

    public static ValidationResult ValidateEstoniaIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != EstoniaIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, EstoniaIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(EstoniaCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidCountryCode);
        }

        // Internal Validation: 7-3-1 Method on BBAN
        // EE BBAN is the last 16 digits (Pos 4-20)
        // This validates the check digit which is the last character of the BBAN.
        string bban = normalized.Substring(4, 16);
        var bbanResult = EstoniaBbanValidator.Validate(bban);
        if (!bbanResult.IsValid)
        {
            return bbanResult;
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
