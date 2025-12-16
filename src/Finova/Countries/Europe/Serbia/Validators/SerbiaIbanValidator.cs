using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.Serbia.Validators;

public class SerbiaIbanValidator : IIbanValidator
{
    public string CountryCode => "RS";
    private const int SerbiaIbanLength = 22;
    private const string SerbiaCountryCode = "RS";

    public ValidationResult Validate(string? iban) => ValidateSerbiaIban(iban);

    public static ValidationResult ValidateSerbiaIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != SerbiaIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidIbanLength, SerbiaIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(SerbiaCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidCountryCode);
        }

        // Structure check: All digits
        for (int i = 2; i < SerbiaIbanLength; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidSerbiaIbanFormat);
            }
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
