using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.Romania.Validators;

public class RomaniaIbanValidator : IIbanValidator
{
    public string CountryCode => "RO";
    private const int RomaniaIbanLength = 24;
    private const string RomaniaCountryCode = "RO";

    public ValidationResult Validate(string? iban) => ValidateRomaniaIban(iban);

    public static ValidationResult ValidateRomaniaIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != RomaniaIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidIbanLength, RomaniaIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(RomaniaCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidCountryCode);
        }

        // Structure check: Alphanumeric
        // Bank Code (Pos 4-8) and Account Number (Pos 8-24) can contain letters.
        for (int i = 4; i < RomaniaIbanLength; i++)
        {
            if (!char.IsLetterOrDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidRomaniaIbanFormat);
            }
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
