using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Monaco.Validators;

public class MonacoIbanValidator : IIbanValidator
{
    public string CountryCode => "MC";
    private const int MonacoIbanLength = 27;
    private const string MonacoCountryCode = "MC";

    public ValidationResult Validate(string? iban) => ValidateMonacoIban(iban);

    public static ValidationResult ValidateMonacoIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != MonacoIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidIbanLengthExpected, MonacoIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(MonacoCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, string.Format(ValidationMessages.InvalidCountryCodeExpected, "MC"));
        }

        // Validate BBAN
        string bban = normalized.Substring(4);
        var bbanResult = MonacoBbanValidator.Validate(bban);
        if (!bbanResult.IsValid)
        {
            return bbanResult;
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
