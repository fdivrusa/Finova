using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Belarus.Validators;

/// <summary>
/// Validator for Belarus Personal Identification Number (Ientifikatsionny nomer).
/// </summary>
public class BelarusNationalIdValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "BY";

    // Format: G DDMMYY R XXX LL C
    // G: 1-6
    // DDMMYY: Date
    // R: Region (A, B, C, K, E, M, H)
    // XXX: Sequence (Digits)
    // LL: Letters (e.g. PB, BA, BI...)
    // C: Checksum (Digit)
    private static readonly Regex FormatRegex = new(@"^[1-6]\d{6}[ABCKEMH]\d{3}[A-Z]{2}\d$", RegexOptions.Compiled);

    /// <summary>
    /// Validates the Belarus Personal ID.
    /// </summary>
    /// <param name="nationalId">The ID to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public ValidationResult Validate(string? nationalId)
    {
        return ValidateStatic(nationalId);
    }

    /// <summary>
    /// Validates the Belarus Personal ID (Static).
    /// </summary>
    /// <param name="nationalId">The ID to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public static ValidationResult ValidateStatic(string? nationalId)
    {
        if (string.IsNullOrWhiteSpace(nationalId))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        string sanitized = InputSanitizer.Sanitize(nationalId) ?? string.Empty;

        if (sanitized.Length != 14)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLength);
        }

        if (!FormatRegex.IsMatch(sanitized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        // Date Validation
        int centuryCode = sanitized[0] - '0';
        int day = int.Parse(sanitized.Substring(1, 2));
        int month = int.Parse(sanitized.Substring(3, 2));
        int yearPart = int.Parse(sanitized.Substring(5, 2));

        int century = 0;
        switch (centuryCode)
        {
            case 1: // Male 19th
            case 2: // Female 19th
                century = 1800;
                break;
            case 3: // Male 20th
            case 4: // Female 20th
                century = 1900;
                break;
            case 5: // Male 21st
            case 6: // Female 21st
                century = 2000;
                break;
        }

        int fullYear = century + yearPart;

        if (!DateHelper.IsValidDate(fullYear, month, day))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        return ValidationResult.Success();
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        var result = Validate(input);
        return result.IsValid ? InputSanitizer.Sanitize(input) : null;
    }
}
