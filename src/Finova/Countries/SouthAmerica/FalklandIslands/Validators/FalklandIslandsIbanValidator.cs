using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.SouthAmerica.FalklandIslands.Validators;

/// <summary>
/// Validator for Falkland Islands IBANs.
/// Falkland Islands IBAN format: FK + 2 check digits + 14 characters BBAN.
/// Length: 18 characters.
/// </summary>
public class FalklandIslandsIbanValidator : IIbanValidator
{
    public string CountryCode => "FK";
    private const int IbanLength = 18;
    private const string CountryCodeVal = "FK";

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

        var bbanResult = FalklandIslandsBbanValidator.Validate(normalized.Substring(4));
        if (!bbanResult.IsValid)
        {
            return bbanResult;
        }

        return IbanHelper.IsValidIban(normalized) ? ValidationResult.Success() : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
