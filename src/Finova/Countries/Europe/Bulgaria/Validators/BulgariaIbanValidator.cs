using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.Bulgaria.Validators;

public class BulgariaIbanValidator : IIbanValidator
{
    public string CountryCode => "BG";
    private const int BulgariaIbanLength = 22;
    private const string BulgariaCountryCode = "BG";

    public ValidationResult Validate(string? iban) => ValidateBulgariaIban(iban);

    public static ValidationResult ValidateBulgariaIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.IbanEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != BulgariaIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidIbanLength, BulgariaIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(BulgariaCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, string.Format(ValidationMessages.InvalidCountryCodeExpected, "BG"));
        }

        // Validate BBAN
        string bban = normalized.Substring(4);
        var bbanResult = BulgariaBbanValidator.Validate(bban);
        if (!bbanResult.IsValid)
        {
            return bbanResult;
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
