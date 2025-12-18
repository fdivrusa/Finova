using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.UnitedKingdom.Validators;

/// <summary>
/// Validator for United Kingdom Company Registration Number (CRN) from Companies House.
/// Format: 8 characters (usually 8 digits, or 2 letters + 6 digits).
/// </summary>
public partial class UnitedKingdomCompanyNumberValidator : ITaxIdValidator
{
    [GeneratedRegex(@"^([A-Z]{2}\d{6}|\d{8})$")]
    private static partial Regex CrnRegex();

    [GeneratedRegex(@"[^\w]")]
    private static partial Regex AlphanumericOnlyRegex();

    // Known prefixes (incomplete list, but covers major ones)
    // SC: Scotland, NI: Northern Ireland, OC: LLP, SO: Scotland LLP, etc.
    private static readonly HashSet<string> ValidPrefixes =
    [
        "SC", "NI", "OC", "SO", "R0", "ZC", "SZ", "LP", "SL", "NC", "NL"
    ];

    public string CountryCode => "GB";

    public ValidationResult Validate(string? input) => ValidateCompanyNumber(input);

    public string? Parse(string? input) => Normalize(input);

    /// <summary>
    /// Validates a UK Company Registration Number (CRN).
    /// </summary>
    public static ValidationResult ValidateCompanyNumber(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = Normalize(number);

        // Pad with leading zeros if numeric and less than 8 digits
        if (normalized.Length < 8 && long.TryParse(normalized, out _))
        {
            normalized = normalized.PadLeft(8, '0');
        }

        if (normalized.Length != 8)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidCrnLength);
        }

        if (!CrnRegex().IsMatch(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidCrnFormat);
        }

        // Prefix validation
        if (!char.IsDigit(normalized[0]))
        {
            string prefix = normalized.Substring(0, 2);
            if (!ValidPrefixes.Contains(prefix))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
            }
        }

        // Note: There is no publicly documented standard checksum algorithm (like Modulo 11) 
        // for all UK Company Registration Numbers that is reliable enough for strict validation.
        // Companies House assigns numbers sequentially. 
        // Therefore, we rely on format and prefix validation.

        return ValidationResult.Success();
    }

    public static string Format(string? number)
    {
        if (!ValidateCompanyNumber(number).IsValid)
        {
            throw new ArgumentException("Invalid CRN", nameof(number));
        }
        var normalized = Normalize(number);
        if (normalized.Length < 8 && long.TryParse(normalized, out _))
        {
            return normalized.PadLeft(8, '0');
        }
        return normalized;
    }

    public static string Normalize(string? number)
    {
        if (string.IsNullOrWhiteSpace(number)) return string.Empty;
        return AlphanumericOnlyRegex().Replace(number, "").ToUpperInvariant();
    }
}
