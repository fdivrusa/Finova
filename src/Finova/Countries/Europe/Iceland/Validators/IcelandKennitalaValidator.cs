using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Enterprise;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Iceland.Validators;

/// <summary>
/// Validator for Iceland Kennitala (National ID).
/// </summary>
public partial class IcelandKennitalaValidator : ITaxIdValidator, INationalIdValidator
{
    [GeneratedRegex(@"^\d{6}-?\d{4}$")]
    private static partial Regex FormatRegex();

    public string CountryCode => "IS";

    public ValidationResult Validate(string? number)
    {
        return ValidateStatic(number);
    }

    /// <summary>
    /// Validates the Iceland Kennitala (Static).
    /// </summary>
    /// <param name="number">The ID to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public static ValidationResult ValidateStatic(string? number)
    {
        return ValidateKennitala(number);
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        var result = Validate(input);
        return result.IsValid ? InputSanitizer.Sanitize(input) : null;
    }

    public static ValidationResult ValidateKennitala(string? kennitala)
    {
        var normalized = EnterpriseNumberNormalizer.Normalize(kennitala, "IS");

        if (string.IsNullOrWhiteSpace(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.KennitalaCannotBeEmpty);
        }

        if (!FormatRegex().IsMatch(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidKennitalaFormat);
        }

        // Checksum Validation
        // Weights: 3, 2, 7, 6, 5, 4, 3, 2
        int[] weights = { 3, 2, 7, 6, 5, 4, 3, 2 };
        int sum = 0;
        for (int i = 0; i < 8; i++)
        {
            sum += (normalized[i] - '0') * weights[i];
        }

        int remainder = sum % 11;
        int checkDigit = 11 - remainder;
        if (checkDigit == 11) checkDigit = 0;

        if (checkDigit == 10)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidKennitalaChecksum);
        }

        if (checkDigit != (normalized[8] - '0'))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidKennitalaChecksum);
        }

        // Date Validation
        int day = int.Parse(normalized.Substring(0, 2));
        int month = int.Parse(normalized.Substring(2, 2));
        int year = int.Parse(normalized.Substring(4, 2));

        // Handle organization numbers (day + 40)
        if (day > 40)
        {
            day -= 40;
        }

        if (day < 1 || day > 31 || month < 1 || month > 12)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidKennitalaDate);
        }

        return ValidationResult.Success();
    }

    public static string? Format(string? instance)
    {
        var normalized = EnterpriseNumberNormalizer.Normalize(instance, "IS");
        if (normalized == null || normalized.Length != 10)
        {
            return normalized;
        }
        return $"{normalized.Substring(0, 6)}-{normalized.Substring(6)}";
    }
}
