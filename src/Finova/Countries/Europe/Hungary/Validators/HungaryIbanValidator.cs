using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.Hungary.Validators;

public class HungaryIbanValidator : IIbanValidator
{
    public string CountryCode => "HU";
    private const int HungaryIbanLength = 28;
    private const string HungaryCountryCode = "HU";

    public ValidationResult Validate(string? iban) => ValidateHungaryIban(iban);

    public static ValidationResult ValidateHungaryIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "IBAN cannot be empty.");
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != HungaryIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, $"Invalid length. Expected {HungaryIbanLength}, got {normalized.Length}.");
        }

        if (!normalized.StartsWith(HungaryCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, "Invalid country code. Expected HU.");
        }

        // Structure check: All body characters must be digits
        for (int i = 2; i < HungaryIbanLength; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Hungary IBAN must contain only digits after the country code.");
            }
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid checksum.");
    }
}
