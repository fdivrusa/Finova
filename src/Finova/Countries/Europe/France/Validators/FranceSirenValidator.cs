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
            if ((digits.Length - i) % 2 == 0) // Even position from right (9th, 7th...) -> Index 0, 2... for length 9
            {
                // For length 9:
                // i=0 (9th from right) -> even pos from right -> double
                // i=1 (8th from right) -> odd pos from right -> add
                // ...
                // Wait, Luhn is usually right-to-left.
                // Rightmost digit is check digit (odd position).
                // Next is even position (double).
                
                // Let's re-verify the logic from SiretValidator which was:
                // if ((digits.Length - i) % 2 == 0) // Even position from right
                
                // For SIRET (14 digits):
                // i=13 (1st from right) -> 14-13=1 (odd) -> add
                // i=12 (2nd from right) -> 14-12=2 (even) -> double
                // Correct.
                
                // For SIREN (9 digits):
                // i=8 (1st from right) -> 9-8=1 (odd) -> add
                // i=7 (2nd from right) -> 9-7=2 (even) -> double
                // Correct.
                
                int doubled = digit * 2;
                if (doubled > 9) doubled -= 9;
                sum += doubled;
            }
            else // Odd position from right
            {
                sum += digit;
            }
        }

        return (sum % 10) == 0;
    }
}
