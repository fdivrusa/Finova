using System.Diagnostics.CodeAnalysis;

using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.Sweden.Validators;

public class SwedenIbanValidator : IIbanValidator
{
    public string CountryCode => "SE";
    private const int SwedenIbanLength = 24;
    private const string SwedenCountryCode = "SE";

    public ValidationResult Validate(string? iban) => ValidateSwedenIban(iban);

    public static ValidationResult ValidateSwedenIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "IBAN cannot be empty.");
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != SwedenIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, $"Invalid length. Expected {SwedenIbanLength}, got {normalized.Length}.");
        }

        if (!normalized.StartsWith(SwedenCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, "Invalid country code. Expected SE.");
        }

        // Structure check: All digits
        // Sweden IBAN body (positions 4 to 24) must be numeric
        for (int i = 2; i < SwedenIbanLength; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Sweden IBAN must contain only digits after the country code.");
            }
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid checksum.");
    }
}
