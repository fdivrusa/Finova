using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.Germany.Validators;

/// <summary>
/// Validator for Germany bank accounts.
/// Germany IBAN format: DE + 2 check digits + 8 bank code + 10 account number (22 characters total).
/// Example : DE89370400440532013000 or formatted: DE89 3704 0044 0532 0130 00
/// </summary>
public class GermanyIbanValidator : IIbanValidator
{
    public string CountryCode => "DE";

    private const int GermanyIbanLength = 22;
    private const string GermanyCountryCode = "DE";

    public ValidationResult Validate(string? iban) => ValidateGermanyIban(iban);

    public static ValidationResult ValidateGermanyIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "IBAN cannot be empty.");
        }
        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != GermanyIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, $"Invalid length. Expected {GermanyIbanLength}, got {normalized.Length}.");
        }

        if (!normalized.StartsWith(GermanyCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, "Invalid country code. Expected DE.");
        }

        for (int i = 2; i < 22; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Germany IBAN must contain only digits after the country code.");
            }
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid checksum.");
    }
}
