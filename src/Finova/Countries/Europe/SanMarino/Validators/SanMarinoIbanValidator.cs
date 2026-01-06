using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.SanMarino.Validators;

public class SanMarinoIbanValidator : IIbanValidator
{
    public string CountryCode => "SM";
    private const int SmIbanLength = 27;
    private const string SmCountryCode = "SM";

    public ValidationResult Validate(string? iban) => ValidateSanMarinoIban(iban);

    public static ValidationResult ValidateSanMarinoIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }
        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != SmIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidIbanLength, SmIbanLength, normalized.Length));
        }
        if (!normalized.StartsWith(SmCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidCountryCode);
        }

        // Validate BBAN (CIN + ABI + CAB + Account)
        // BBAN starts at index 4
        string bban = normalized.Substring(4);
        var bbanResult = SanMarinoBbanValidator.Validate(bban);
        if (!bbanResult.IsValid)
        {
            return bbanResult;
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
