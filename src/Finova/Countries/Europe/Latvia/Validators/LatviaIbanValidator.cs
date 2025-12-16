using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.Latvia.Validators;

public class LatviaIbanValidator : IIbanValidator
{
    public string CountryCode => "LV";
    private const int LatviaIbanLength = 21;
    private const string LatviaCountryCode = "LV";

    public ValidationResult Validate(string? iban) => ValidateLatviaIban(iban);

    public static ValidationResult ValidateLatviaIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != LatviaIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, LatviaIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(LatviaCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidLatviaCountryCode);
        }

        // Structure check: Alphanumeric
        // Bank Code (4 chars) + Account (13 chars)
        for (int i = 4; i < LatviaIbanLength; i++)
        {
            if (!char.IsLetterOrDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.LatviaIbanMustBeAlphanumeric);
            }
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
