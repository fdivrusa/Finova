using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.Lithuania.Validators;

public class LithuaniaIbanValidator : IIbanValidator
{
    public string CountryCode => "LT";
    private const int LithuaniaIbanLength = 20;
    private const string LithuaniaCountryCode = "LT";

    public ValidationResult Validate(string? iban) => ValidateLithuaniaIban(iban);

    public static ValidationResult ValidateLithuaniaIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "IBAN cannot be empty.");
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != LithuaniaIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, $"Invalid length. Expected {LithuaniaIbanLength}, got {normalized.Length}.");
        }

        if (!normalized.StartsWith(LithuaniaCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, "Invalid country code. Expected LT.");
        }

        // Structure check: Digits only
        for (int i = 2; i < LithuaniaIbanLength; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Lithuania IBAN must contain only digits after the country code.");
            }
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid checksum.");
    }
}
