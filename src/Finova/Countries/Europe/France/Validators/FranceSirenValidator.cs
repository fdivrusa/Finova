using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Enterprise;

namespace Finova.Countries.Europe.France.Validators;

/// <summary>
/// Validator for French SIREN numbers (Système d'Identification du Répertoire des Entreprises).
/// Format: 9 digits.
/// Checksum: Luhn algorithm.
/// </summary>
public partial class FranceSirenValidator : IEnterpriseValidator
{
    private const string CountryCodePrefix = "FR";
    private const int SirenLength = 9;

    [GeneratedRegex(@"^\d{9}$")]
    private static partial Regex SirenRegex();

    public string CountryCode => CountryCodePrefix;

    public ValidationResult Validate(string? number) => ValidateSiren(number);

    public ValidationResult Validate(string? number, string countryCode)
    {
        return countryCode.Equals(CountryCodePrefix, StringComparison.OrdinalIgnoreCase)
            ? Validate(number)
            : ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, $"Country code {countryCode} is not supported by this validator.");
    }

    public ValidationResult Validate(string? number, EnterpriseNumberType type)
    {
        return type == EnterpriseNumberType.FranceSiren
            ? Validate(number)
            : ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, $"Enterprise number type {type} is not supported by this validator.");
    }

    public string? Parse(string? number) => number?.Trim().Replace(" ", "").Replace(".", "");

    public static ValidationResult ValidateSiren(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "SIREN number cannot be empty.");
        }

        var normalized = number.Trim().Replace(" ", "").Replace(".", "");

        if (!SirenRegex().IsMatch(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid format. Expected 9 digits.");
        }

        if (!LuhnCheck(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid checksum (Luhn).");
        }

        return ValidationResult.Success();
    }

    private static bool LuhnCheck(string digits)
    {
        int sum = 0;
        for (int i = 0; i < digits.Length; i++)
        {
            int digit = digits[i] - '0';
            if ((digits.Length - i) % 2 == 0)
            {
                int doubled = digit * 2;
                if (doubled > 9) doubled -= 9;
                sum += doubled;
            }
            else
            {
                sum += digit;
            }
        }

        return (sum % 10) == 0;
    }
}
