using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Luxembourg.Validators;

/// <summary>
/// Validator for Luxembourg RCS (Registre de Commerce et des Sociétés) numbers.
/// RCS numbers are assigned to companies registered in the Luxembourg Trade and Companies Register.
/// Format: B + 6 digits (e.g., B123456) for commercial companies, or other prefixes for different entity types.
/// </summary>
public partial class LuxembourgRcsValidator : ITaxIdValidator
{
    private const string CountryPrefix = "LU";

    public string CountryCode => CountryPrefix;

    public ValidationResult Validate(string? instance) => ValidateRcs(instance);

    public string? Parse(string? instance) => Normalize(instance);

    /// <summary>
    /// Validates a Luxembourg RCS number.
    /// Formats accepted:
    /// - B followed by 1-6 digits (commercial companies): B1, B12, ..., B123456
    /// - A followed by digits (civil companies)
    /// - C followed by digits (branches of foreign companies)
    /// - D followed by digits (simplified joint-stock companies)
    /// - E followed by digits (European Economic Interest Groupings)
    /// - F followed by digits (private foundations)
    /// - G followed by digits (agricultural associations)
    /// - H followed by digits (hospital associations)
    /// - J followed by digits (non-profit organizations)
    /// - R followed by digits (cooperatives)
    /// - S followed by digits (European companies - SE)
    /// - T followed by digits (European cooperative societies - SCE)
    /// </summary>
    public static ValidationResult ValidateRcs(string? rcs)
    {
        if (string.IsNullOrWhiteSpace(rcs))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var cleaned = Normalize(rcs);

        if (string.IsNullOrEmpty(cleaned))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidLuxembourgRcsFormat);
        }

        // Validate prefix is a valid RCS type letter
        char prefix = cleaned[0];
        if (!IsValidRcsPrefix(prefix))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidLuxembourgRcsFormat);
        }

        // Validate numeric part (1-6 digits)
        string numericPart = cleaned[1..];
        if (numericPart.Length < 1 || numericPart.Length > 6)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidLuxembourgRcsFormat);
        }

        if (!int.TryParse(numericPart, out _))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidLuxembourgRcsFormat);
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Checks if the prefix is a valid RCS entity type.
    /// </summary>
    private static bool IsValidRcsPrefix(char prefix)
    {
        return prefix switch
        {
            'A' => true, // Civil companies (sociétés civiles)
            'B' => true, // Commercial companies (sociétés commerciales)
            'C' => true, // Branches of foreign companies
            'D' => true, // Simplified joint-stock companies (SAS)
            'E' => true, // European Economic Interest Groupings (GEIE)
            'F' => true, // Private foundations
            'G' => true, // Agricultural associations
            'H' => true, // Hospital associations
            'J' => true, // Non-profit organizations (ASBL)
            'R' => true, // Cooperatives
            'S' => true, // European companies (SE)
            'T' => true, // European cooperative societies (SCE)
            _ => false
        };
    }

    /// <summary>
    /// Normalizes an RCS number by removing whitespace and converting to uppercase.
    /// </summary>
    public static string Normalize(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return string.Empty;
        }

        // Remove all whitespace and non-alphanumeric characters except the letter prefix
        var cleaned = number.Trim().ToUpperInvariant()
            .Replace(" ", "")
            .Replace("-", "")
            .Replace(".", "");

        // Remove LU prefix if present
        if (cleaned.StartsWith(CountryPrefix))
        {
            cleaned = cleaned[2..];
        }

        // Remove "RCS" prefix if present (sometimes written as "RCS B123456")
        if (cleaned.StartsWith("RCS"))
        {
            cleaned = cleaned[3..].TrimStart();
        }

        return cleaned;
    }
}
