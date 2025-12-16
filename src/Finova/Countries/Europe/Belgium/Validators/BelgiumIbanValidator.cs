using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.Belgium.Validators;

/// <summary>
/// Validator for Belgian IBAN bank accounts.
/// Belgian IBAN format: BE + 2 check digits + 3 Bank + 7 Account + 2 National Check.
/// Total: 16 characters.
/// </summary>
public class BelgiumIbanValidator : IIbanValidator
{
    public string CountryCode => "BE";

    private const int BelgianIbanLength = 16;
    private const string BelgianCountryCode = "BE";

    public ValidationResult Validate(string? iban) => ValidateBelgiumIban(iban);

    public static ValidationResult ValidateBelgiumIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != BelgianIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, BelgianIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(BelgianCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidCountryCode);
        }

        // Internal BBAN Validation (Specific to Belgium)
        // Extract the last 12 digits (Positions 4 to 16)
        string bban = normalized.Substring(4, 12);

        var bbanResult = BelgiumBbanValidator.Validate(bban);
        if (!bbanResult.IsValid)
        {
            return bbanResult;
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
