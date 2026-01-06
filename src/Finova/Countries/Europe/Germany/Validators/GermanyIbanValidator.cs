using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Germany.Validators;

/// <summary>
/// Validator for Germany bank accounts.
/// Germany IBAN format: DE + 2 check digits + 8 bank code + 10 account number (22 characters total).
/// Example : DE89370400440532013000 or formatted: DE89 3704 0044 0532 0130 00
/// </summary>
public class GermanyIbanValidator : IIbanValidator
{
    public string CountryCode => "DE";

    private const int GermanyIbanLength = 22;
    private const string GermanyCountryCode = "DE";

    public ValidationResult Validate(string? iban) => ValidateGermanyIban(iban);

    public static ValidationResult ValidateGermanyIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }
        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != GermanyIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, GermanyIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(GermanyCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidGermanyCountryCode);
        }

        // Validate BBAN
        string bban = normalized.Substring(4);
        var bbanResult = GermanyBbanValidator.Validate(bban);
        if (!bbanResult.IsValid)
        {
            return bbanResult;
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
