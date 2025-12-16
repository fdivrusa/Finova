using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.Andorra.Validators;

public class AndorraIbanValidator : IIbanValidator
{
    public string CountryCode => "AD";
    private const int AndorraIbanLength = 24;
    private const string AndorraCountryCode = "AD";

    public ValidationResult Validate(string? iban) => ValidateAndorraIban(iban);

    public static ValidationResult ValidateAndorraIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.IbanEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != AndorraIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidIbanLength, AndorraIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(AndorraCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, string.Format(ValidationMessages.InvalidCountryCodeExpected, AndorraCountryCode));
        }

        // Structure check:
        // Bank (4) and Branch (4) are typically numeric or alphanumeric?
        // Standard allows alphanumeric for Andorra account part, usually numeric for bank/branch.
        // Let's enforce Alphanumeric globally for safety as per generic registry.
        for (int i = 4; i < AndorraIbanLength; i++)
        {
            if (!char.IsLetterOrDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, string.Format(ValidationMessages.InvalidIbanFormatAlphanumeric, "Andorra"));
            }
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
