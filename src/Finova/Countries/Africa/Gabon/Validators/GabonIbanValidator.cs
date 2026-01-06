using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Africa.Gabon.Validators;

/// <summary>
/// Validator for Gabon IBANs.
/// Gabon IBAN format: GA + 2 check digits + 23 digits BBAN.
/// Length: 27 characters.
/// </summary>
public class GabonIbanValidator : IIbanValidator
{
    public string CountryCode => "GA";
    private const int IbanLength = 27;
    private const string CountryCodeVal = "GA";

    public ValidationResult Validate(string? iban) => ValidateIban(iban);

    public static ValidationResult ValidateIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != IbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, IbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(CountryCodeVal, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidCountryCode);
        }

        var bbanResult = GabonBbanValidator.Validate(normalized.Substring(4));
        if (!bbanResult.IsValid)
        {
            return bbanResult;
        }

        return IbanHelper.IsValidIban(normalized) ? ValidationResult.Success() : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
