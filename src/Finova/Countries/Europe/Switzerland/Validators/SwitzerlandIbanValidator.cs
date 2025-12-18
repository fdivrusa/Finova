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
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != SwitzerlandIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, SwitzerlandIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(SwitzerlandCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, string.Format(ValidationMessages.InvalidCountryCodeExpected, "CH"));
        }

        // Validate BBAN
        string bban = normalized.Substring(4);
        var bbanResult = SwitzerlandBbanValidator.Validate(bban);
        if (!bbanResult.IsValid)
        {
            return bbanResult;
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
