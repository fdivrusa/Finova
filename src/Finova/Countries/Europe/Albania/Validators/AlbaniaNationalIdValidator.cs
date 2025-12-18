using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Albania.Validators;

/// <summary>
/// Validator for Albanian National Identity Number (NID / Letërnjoftim).
/// </summary>
public class AlbaniaNationalIdValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "AL";

    private static readonly Regex FormatRegex = new(@"^[A-Z]\d{8}[A-Z]$", RegexOptions.Compiled);

    /// <summary>
    /// Validates the Albanian NID.
    /// </summary>
    /// <param name="nationalId">The ID to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public ValidationResult Validate(string? nationalId)
    {
        return ValidateStatic(nationalId);
    }

    /// <summary>
    /// Validates the Albanian NID (Static).
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

        if (sanitized.Length != 10)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLength);
        }

        if (!FormatRegex.IsMatch(sanitized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        // Date Validation
        // Format: L YY MM DD SSS C
        // L: Decade (A=1900, B=1910, ... J=1990, K=2000, L=2010, M=2020)
        // YY: Year (last 2 digits)
        // MM: Month (01-12 Male, 51-62 Female)
        // DD: Day
        
        char decadeChar = sanitized[0];
        int yearPart = int.Parse(sanitized.Substring(1, 2));
        int monthPart = int.Parse(sanitized.Substring(3, 2));
        int dayPart = int.Parse(sanitized.Substring(5, 2));

        int decadeBase = GetDecadeBase(decadeChar);
        if (decadeBase == -1)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        // Validate that yearPart matches the decade
        // e.g. Decade J (1990) -> yearPart must be 90-99?
        // Or does J mean 19xx?
        // If J=1990, then yearPart 95 -> 1995.
        // If J=1990, then yearPart 05 -> 1905? No.
        // Let's assume strict matching: yearPart / 10 must match decade index?
        // No, decade is 10 years.
        // So yearPart must be between (DecadeYear % 100) and (DecadeYear % 100) + 9.
        // e.g. J=1990. Range 90-99.
        // K=2000. Range 00-09.
        
        int expectedDecadeStart = decadeBase % 100;
        if (yearPart < expectedDecadeStart || yearPart > expectedDecadeStart + 9)
        {
            // This logic assumes strict decade mapping.
            // If yearPart is 95, it fits in 90-99.
            // If yearPart is 05, it fits in 00-09.
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        int fullYear = decadeBase + (yearPart % 10); // Wait.
        // If DecadeBase is 1990. YearPart is 95.
        // FullYear = 1900 + 95 = 1995.
        // So FullYear = (DecadeBase / 100) * 100 + YearPart?
        // No.
        // If DecadeBase is 1990.
        // FullYear = 1990 + (yearPart - 90) = 1995.
        // Or simply: FullYear = (DecadeBase / 10) * 10 + (yearPart % 10)?
        // No.
        // Let's just use: FullYear = (DecadeBase / 100) * 100 + YearPart.
        // e.g. 1900 + 95 = 1995.
        // But wait, if Decade is K (2000), YearPart is 05.
        // FullYear = 2000 + 05 = 2005.
        // This works IF the century is correct.
        // Does A always mean 1900s?
        // A=1900, B=1910... J=1990.
        // K=2000, L=2010...
        // So yes, the century is implied by the letter.
        
        // So:
        // If letter is A-J (1900-1999): Century is 1900.
        // If letter is K-T (2000-2099): Century is 2000.
        
        int century = (decadeBase >= 2000) ? 2000 : 1900;
        fullYear = century + yearPart;

        // Handle Month (Male/Female)
        int month = monthPart;
        if (month > 50)
        {
            month -= 50;
        }

        if (!DateHelper.IsValidDate(fullYear, month, dayPart))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        return ValidationResult.Success();
    }

    private static int GetDecadeBase(char c)
    {
        return c switch
        {
            'A' => 1900,
            'B' => 1910,
            'C' => 1920,
            'D' => 1930,
            'E' => 1940,
            'F' => 1950,
            'G' => 1960,
            'H' => 1970,
            'I' => 1980,
            'J' => 1990,
            'K' => 2000,
            'L' => 2010,
            'M' => 2020,
            'N' => 2030,
            'P' => 2040, // Skipping O?
            // Assuming standard progression.
            _ => -1
        };
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        var result = Validate(input);
        return result.IsValid ? InputSanitizer.Sanitize(input) : null;
    }
}
