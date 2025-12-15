using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Enterprise;

namespace Finova.Countries.Europe.Spain.Validators;

/// <summary>
/// Validator for Spanish CIF (Certificado de Identificación Fiscal).
/// Format: 1 Letter + 7 Digits + 1 Control Character (Digit or Letter).
/// </summary>
public partial class SpainCifValidator : IEnterpriseValidator
{
    [GeneratedRegex(@"^[ABCDEFGHJKLMNPQRSUVW]\d{7}[0-9A-J]$")]
    private static partial Regex CifRegex();

    [GeneratedRegex(@"[^\w]")]
    private static partial Regex AlphanumericOnlyRegex();

    public string CountryCode => "ES";

    public ValidationResult Validate(string? instance) => ValidateCif(instance);

    public string? Parse(string? instance) => Normalize(instance);

    /// <summary>
    /// Validates a Spanish CIF.
    /// </summary>
    public static ValidationResult ValidateCif(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "CIF cannot be empty.");
        }

        var normalized = Normalize(number);

        if (!CifRegex().IsMatch(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid CIF format.");
        }

        char entityType = normalized[0];
        string digitsStr = normalized.Substring(1, 7);
        char providedControl = normalized[8];

        // Weights for CIF: 2, 1, 2, 1, 2, 1, 2
        // Note: ChecksumHelper.CalculateLuhnStyleWeightedSum sums digits of products > 9.
        // This matches CIF algorithm: "Sum of (odd-position digits * 2) -> sum digits of result."
        // Odd positions in CIF (1st, 3rd...) correspond to indices 0, 2, 4, 6.
        // Even positions (2nd, 4th...) are added directly (weight 1).
        // So weights are: 2, 1, 2, 1, 2, 1, 2.
        int[] weights = [2, 1, 2, 1, 2, 1, 2];

        int sum = ChecksumHelper.CalculateLuhnStyleWeightedSum(digitsStr, weights);

        int lastDigitOfSum = sum % 10;
        int controlDigit = (10 - lastDigitOfSum) % 10;

        char expectedControlDigit = (char)('0' + controlDigit);
        char expectedControlLetter = (char)('A' + (controlDigit == 0 ? 9 : controlDigit - 1));

        bool expectsLetter = "KPQS".Contains(entityType);
        bool expectsDigit = "ABEH".Contains(entityType);

        if (expectsLetter)
        {
            return providedControl == expectedControlLetter
                ? ValidationResult.Success()
                : ValidationResult.Failure(ValidationErrorCode.InvalidCheckDigit, "Invalid CIF check letter.");
        }
        else if (expectsDigit)
        {
            return providedControl == expectedControlDigit
                ? ValidationResult.Success()
                : ValidationResult.Failure(ValidationErrorCode.InvalidCheckDigit, "Invalid CIF check digit.");
        }
        else
        {
            return (providedControl == expectedControlDigit || providedControl == expectedControlLetter)
                ? ValidationResult.Success()
                : ValidationResult.Failure(ValidationErrorCode.InvalidCheckDigit, "Invalid CIF check character.");
        }
    }

    public static string Format(string? number)
    {
        if (!ValidateCif(number).IsValid)
        {
            throw new ArgumentException("Invalid CIF", nameof(number));
        }
        return Normalize(number);
    }

    public static string Normalize(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return string.Empty;
        }

        return AlphanumericOnlyRegex().Replace(number, "").ToUpperInvariant();
    }
}
