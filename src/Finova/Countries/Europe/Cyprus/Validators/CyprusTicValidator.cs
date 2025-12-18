using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Enterprise;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Cyprus.Validators;

/// <summary>
/// Validator for Cyprus TIC (Tax Identification Code).
/// </summary>
public partial class CyprusTicValidator : ITaxIdValidator
{
    [GeneratedRegex(@"^\d{8}[A-Z]$")]
    private static partial Regex TicRegex();

    private static readonly int[] CyprusMap = [1, 0, 5, 7, 9, 13, 15, 17, 19, 21];

    public string CountryCode => "CY";

    public ValidationResult Validate(string? number)
    {
        return ValidateTic(number);
    }

    public string? Parse(string? number)
    {
        return EnterpriseNumberNormalizer.Normalize(number, CountryCode);
    }

    public static ValidationResult ValidateTic(string? tic)
    {
        var normalized = EnterpriseNumberNormalizer.Normalize(tic, "CY");

        if (string.IsNullOrWhiteSpace(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.CyprusTicEmpty);
        }

        if (!TicRegex().IsMatch(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.CyprusTicInvalidFormat);
        }

        // Checksum Validation
        // Format: 12345678 L
        var digits = normalized.AsSpan(0, 8);
        char expectedLetter = normalized[8];

        int sum = 0;

        for (int i = 0; i < 8; i++)
        {
            int digit = digits[i] - '0';

            // Even positions (0, 2, 4...) use mapping
            if (i % 2 == 0)
            {
                sum += CyprusMap[digit];
            }
            else
            {
                sum += digit;
            }
        }

        int remainder = sum % 26;
        char calculatedLetter = (char)(remainder + 'A');

        if (calculatedLetter != expectedLetter)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.CyprusTicInvalidChecksum);
        }

        return ValidationResult.Success();
    }

    public static string? Format(string? instance)
    {
        return EnterpriseNumberNormalizer.Normalize(instance, "CY");
    }
}
