using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.France.Validators;

/// <summary>
/// Validator for French SIREN numbers (Système d'Identification du Répertoire des Entreprises).
/// Format: 9 digits.
/// Checksum: Luhn algorithm.
/// </summary>
public partial class FranceSirenValidator : ITaxIdValidator
{
    private const string CountryCodePrefix = "FR";
    private const int SirenLength = 9;

    [GeneratedRegex(@"^\d{9}$")]
    private static partial Regex SirenRegex();

    public string CountryCode => CountryCodePrefix;

    public ValidationResult Validate(string? number) => ValidateSiren(number);



    public string? Parse(string? number) => number?.Trim().Replace(" ", "").Replace(".", "");

    public static ValidationResult ValidateSiren(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.FranceSirenEmpty);
        }

        var normalized = number.Trim().Replace(" ", "").Replace(".", "");

        if (!SirenRegex().IsMatch(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.FranceSirenInvalidFormat);
        }

        if (!LuhnCheck(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.FranceSirenInvalidChecksum);
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
