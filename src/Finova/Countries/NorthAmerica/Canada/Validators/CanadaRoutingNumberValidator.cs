using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.NorthAmerica.Canada.Validators;

/// <summary>
/// Validates Canadian Payments Association (CPA) Routing Numbers.
/// Supports two formats:
/// <list type="bullet">
///   <item><description>EFT (Electronic Funds Transfer) format: 0YYYXXXXX (9 digits) - Used for electronic transactions. Starts with 0, followed by 3-digit institution code, then 5-digit transit number.</description></item>
///   <item><description>MICR (Paper cheque) format: XXXXX-YYY (8 digits) - Used on paper cheques. 5-digit transit number, hyphen (optional), 3-digit institution code.</description></item>
/// </list>
/// </summary>
public partial class CanadaRoutingNumberValidator : IBankRoutingValidator, IBankRoutingParser
{
    // EFT format: 0 + 3-digit institution + 5-digit transit = 9 digits
    [GeneratedRegex(@"^0\d{8}$", RegexOptions.Compiled)]
    private static partial Regex EftPattern();

    // MICR format: 5-digit transit + optional hyphen + 3-digit institution = 8 digits
    [GeneratedRegex(@"^\d{5}-?\d{3}$", RegexOptions.Compiled)]
    private static partial Regex MicrPattern();

    /// <inheritdoc/>
    public string CountryCode => "CA";

    /// <inheritdoc/>
    public ValidationResult Validate(string? input) => ValidateStatic(input);

    /// <summary>
    /// Validates the Canadian Routing Number.
    /// Accepts both EFT (9 digits: 0YYYXXXXX) and MICR (8 digits: XXXXX-YYY) formats.
    /// </summary>
    /// <param name="input">The routing number to validate.</param>
    /// <returns>A ValidationResult.</returns>
    public static ValidationResult ValidateStatic(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        string sanitized = input.Replace(" ", "");

        // Check EFT format first (9 digits starting with 0)
        if (EftPattern().IsMatch(sanitized.Replace("-", "")))
        {
            return ValidationResult.Success();
        }

        // Check MICR format (8 digits, optionally with hyphen)
        if (MicrPattern().IsMatch(sanitized))
        {
            return ValidationResult.Success();
        }

        return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidCanadaRoutingNumberFormat);
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        if (Validate(input).IsValid)
        {
            return NormalizeToEft(input);
        }
        return null;
    }

    /// <summary>
    /// Normalizes the routing number to EFT format (0YYYXXXXX).
    /// </summary>
    /// <param name="input">The routing number in either format.</param>
    /// <returns>The routing number in EFT format, or null if invalid.</returns>
    public static string? NormalizeToEft(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return null;
        }

        var sanitized = input.Replace(" ", "").Replace("-", "");

        // Already in EFT format
        if (sanitized.Length == 9 && sanitized[0] == '0')
        {
            return sanitized;
        }

        // Convert MICR to EFT: XXXXX-YYY -> 0YYYXXXXX
        if (sanitized.Length == 8)
        {
            var transitNumber = sanitized[..5];     // First 5 digits
            var institutionCode = sanitized[5..8];  // Last 3 digits
            return $"0{institutionCode}{transitNumber}";
        }

        return null;
    }

    /// <summary>
    /// Converts the routing number to MICR format (XXXXX-YYY).
    /// </summary>
    /// <param name="input">The routing number in either format.</param>
    /// <returns>The routing number in MICR format with hyphen, or null if invalid.</returns>
    public static string? ConvertToMicr(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return null;
        }

        var sanitized = input.Replace(" ", "").Replace("-", "");

        // Convert EFT to MICR: 0YYYXXXXX -> XXXXX-YYY
        if (sanitized.Length == 9 && sanitized[0] == '0')
        {
            var institutionCode = sanitized[1..4];  // Digits 2-4
            var transitNumber = sanitized[4..9];    // Digits 5-9
            return $"{transitNumber}-{institutionCode}";
        }

        // Already in MICR format (normalize with hyphen)
        if (sanitized.Length == 8)
        {
            return $"{sanitized[..5]}-{sanitized[5..8]}";
        }

        return null;
    }

    /// <inheritdoc/>
    public BankRoutingDetails? ParseRoutingNumber(string? routingNumber)
    {
        var result = Validate(routingNumber);
        if (!result.IsValid || string.IsNullOrEmpty(routingNumber))
        {
            return null;
        }

        // Normalize to EFT format for consistent parsing
        var eftFormat = NormalizeToEft(routingNumber);
        if (string.IsNullOrEmpty(eftFormat))
        {
            return null;
        }

        // EFT Format: 0YYYXXXXX
        // YYY = Institution (positions 1-3)
        // XXXXX = Transit (positions 4-8)

        return new BankRoutingDetails
        {
            RoutingNumber = eftFormat,
            CountryCode = "CA",
            BankCode = eftFormat.Substring(1, 3),       // Institution code
            BranchCode = eftFormat.Substring(4, 5)      // Transit number
        };
    }
}
