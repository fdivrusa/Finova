using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.France.Validators;

/// <summary>
/// Validator for French SIRET numbers (Système d'Identification du Répertoire des Établissements).
/// Format: 14 digits (9 digits SIREN + 5 digits NIC).
/// Checksum: Luhn algorithm.
/// </summary>
public partial class FranceSiretValidator : ITaxIdValidator
{
    private const string CountryCodePrefix = "FR";

    [GeneratedRegex(@"^\d{14}$")]
    private static partial Regex SiretRegex();

    public string CountryCode => CountryCodePrefix;

    public ValidationResult Validate(string? number) => ValidateSiret(number);



    public string? Parse(string? number) => number?.Trim().Replace(" ", "").Replace(".", "");

    public static ValidationResult ValidateSiret(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.SiretCannotBeEmpty);
        }

        var normalized = number.Trim().Replace(" ", "").Replace(".", "");

        if (!SiretRegex().IsMatch(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidSiretFormat);
        }

        if (!LuhnCheck(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidSiretChecksum);
        }

        return ValidationResult.Success();
    }

    private static bool LuhnCheck(string digits)
    {
        int sum = 0;
        for (int i = 0; i < digits.Length; i++)
        {
            int digit = digits[i] - '0';
            if ((digits.Length - i) % 2 == 0) // Even position from right (14th, 12th...) -> Index 0, 2...
            {
                int doubled = digit * 2;
                if (doubled > 9) doubled -= 9;
                sum += doubled;
            }
            else // Odd position from right -> Index 1, 3...
            {
                sum += digit;
            }
        }

        return (sum % 10) == 0;
    }

    public static France.Models.FranceSiretDetails? GetSiretDetails(string? number)
    {
        var result = ValidateSiret(number);
        if (!result.IsValid)
        {
            return null;
        }

        var normalized = number!.Trim().Replace(" ", "").Replace(".", "");

        return new France.Models.FranceSiretDetails
        {
            Siret = normalized,
            Siren = normalized.Substring(0, 9),
            Nic = normalized.Substring(9, 5),
            IsValid = true
        };
    }
}
