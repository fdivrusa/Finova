using System.Text.RegularExpressions;
using Finova.Core.Common;

namespace Finova.Core.Identifiers;

/// <summary>
/// Details parsed from an ISIN (International Securities Identification Number).
/// </summary>
public class IsinDetails
{
    /// <summary>
    /// The full normalized ISIN code (12 characters).
    /// </summary>
    public string Isin { get; init; } = string.Empty;

    /// <summary>
    /// The ISO 3166-1 alpha-2 country code (2 characters).
    /// </summary>
    public string CountryCode { get; init; } = string.Empty;

    /// <summary>
    /// The National Securities Identifying Number (NSIN) (9 characters).
    /// This can be a CUSIP (US/CA), SEDOL (GB), WKN (DE), etc.
    /// </summary>
    public string Nsin { get; init; } = string.Empty;

    /// <summary>
    /// The check digit (1 character).
    /// </summary>
    public char CheckDigit { get; init; }

    /// <summary>
    /// Whether the ISIN is valid.
    /// </summary>
    public bool IsValid { get; init; }
}

/// <summary>
/// Interface for ISIN validation.
/// </summary>
public interface IIsinValidator
{
    /// <summary>
    /// Validates an ISIN (International Securities Identification Number).
    /// </summary>
    ValidationResult Validate(string? isin);

    /// <summary>
    /// Parses an ISIN and returns detailed information.
    /// </summary>
    IsinDetails? Parse(string? isin);
}

/// <summary>
/// Validator for ISIN (International Securities Identification Number) based on ISO 6166.
/// An ISIN uniquely identifies a security (stocks, bonds, derivatives, etc.).
/// </summary>
/// <remarks>
/// Format: 2-letter country code + 9-character NSIN + 1 check digit = 12 characters total.
/// The check digit is calculated using a modified Luhn algorithm (double-add-double).
/// </remarks>
/// <example>
/// <code>
/// // Validate an ISIN
/// var result = IsinValidator.Validate("US0378331005"); // Apple Inc.
/// if (result.IsValid)
/// {
///     Console.WriteLine("Valid ISIN");
/// }
/// 
/// // Parse an ISIN
/// var details = IsinValidator.Parse("US0378331005");
/// Console.WriteLine($"Country: {details?.CountryCode}"); // US
/// Console.WriteLine($"NSIN: {details?.Nsin}"); // 037833100
/// </code>
/// </example>
public partial class IsinValidator : IIsinValidator
{
    [GeneratedRegex(@"^[A-Z]{2}[A-Z0-9]{9}[0-9]$")]
    private static partial Regex IsinRegex();

    /// <summary>
    /// Validates an ISIN (International Securities Identification Number).
    /// </summary>
    /// <param name="isin">The ISIN string to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public static ValidationResult Validate(string? isin)
    {
        if (string.IsNullOrWhiteSpace(isin))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = Normalize(isin);

        if (normalized.Length != 12)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidIsinLength);
        }

        if (!IsinRegex().IsMatch(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidIsinFormat);
        }

        // Validate checksum using Luhn algorithm on numeric conversion
        if (!ValidateChecksum(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidIsinChecksum);
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Parses an ISIN and returns detailed information.
    /// </summary>
    /// <param name="isin">The ISIN string to parse.</param>
    /// <returns>An <see cref="IsinDetails"/> object if valid, otherwise null.</returns>
    public static IsinDetails? Parse(string? isin)
    {
        if (string.IsNullOrWhiteSpace(isin))
        {
            return null;
        }

        var normalized = Normalize(isin);

        if (normalized.Length != 12)
        {
            return null;
        }

        var isValid = IsinRegex().IsMatch(normalized) && ValidateChecksum(normalized);

        return new IsinDetails
        {
            Isin = normalized,
            CountryCode = normalized[..2],
            Nsin = normalized[2..11],
            CheckDigit = normalized[11],
            IsValid = isValid
        };
    }

    /// <summary>
    /// Calculates the check digit for an ISIN base (11 characters without check digit).
    /// </summary>
    /// <param name="isinBase">The first 11 characters of the ISIN.</param>
    /// <returns>The calculated check digit (0-9).</returns>
    public static char CalculateCheckDigit(string isinBase)
    {
        if (string.IsNullOrWhiteSpace(isinBase) || isinBase.Length != 11)
        {
            throw new ArgumentException("ISIN base must be exactly 11 characters.", nameof(isinBase));
        }

        var normalized = isinBase.ToUpperInvariant();
        var digits = ConvertToDigits(normalized);
        var checkDigit = CalculateLuhnCheckDigit(digits);
        return (char)('0' + checkDigit);
    }

    /// <summary>
    /// Generates a valid ISIN from country code and NSIN.
    /// </summary>
    /// <param name="countryCode">The 2-letter ISO 3166-1 alpha-2 country code.</param>
    /// <param name="nsin">The 9-character National Securities Identifying Number.</param>
    /// <returns>A valid 12-character ISIN.</returns>
    public static string Generate(string countryCode, string nsin)
    {
        if (string.IsNullOrWhiteSpace(countryCode) || countryCode.Length != 2)
        {
            throw new ArgumentException("Country code must be exactly 2 characters.", nameof(countryCode));
        }

        if (string.IsNullOrWhiteSpace(nsin) || nsin.Length != 9)
        {
            throw new ArgumentException("NSIN must be exactly 9 characters.", nameof(nsin));
        }

        var isinBase = (countryCode + nsin).ToUpperInvariant();
        var checkDigit = CalculateCheckDigit(isinBase);
        return isinBase + checkDigit;
    }

    private static string Normalize(string isin)
    {
        return isin.Trim().ToUpperInvariant().Replace(" ", "").Replace("-", "");
    }

    private static bool ValidateChecksum(string isin)
    {
        var digits = ConvertToDigits(isin);
        return ValidateLuhn(digits);
    }

    private static string ConvertToDigits(string isin)
    {
        var sb = new System.Text.StringBuilder(isin.Length * 2);
        foreach (char c in isin)
        {
            if (char.IsDigit(c))
            {
                sb.Append(c);
            }
            else if (char.IsLetter(c))
            {
                // A=10, B=11, ..., Z=35
                sb.Append(c - 'A' + 10);
            }
        }
        return sb.ToString();
    }

    private static bool ValidateLuhn(string digits)
    {
        // ISIN uses the Luhn algorithm
        // Process from RIGHT to LEFT, doubling every SECOND digit (positions 1, 3, 5... from right)
        int sum = 0;

        for (int i = digits.Length - 1; i >= 0; i--)
        {
            int posFromRight = digits.Length - 1 - i;
            int digit = digits[i] - '0';

            // Double at odd positions from right (1, 3, 5...)
            if (posFromRight % 2 == 1)
            {
                digit *= 2;
                if (digit > 9)
                {
                    digit -= 9;
                }
            }

            sum += digit;
        }

        return sum % 10 == 0;
    }

    private static int CalculateLuhnCheckDigit(string digits)
    {
        // To calculate check digit: the check digit will be at position 0 from right (not doubled)
        // So we need to find which digit at position 0 makes sum % 10 == 0
        // Current digits will be at positions 1, 2, 3... from right after adding check digit
        int sum = 0;

        for (int i = digits.Length - 1; i >= 0; i--)
        {
            // After adding check digit, this digit will be at position (digits.Length - i) from right
            int posFromRight = digits.Length - i;
            int digit = digits[i] - '0';

            // Double at odd positions from right (1, 3, 5...)
            if (posFromRight % 2 == 1)
            {
                digit *= 2;
                if (digit > 9)
                {
                    digit -= 9;
                }
            }

            sum += digit;
        }

        // Check digit is at position 0 (not doubled), find value that makes sum divisible by 10
        return (10 - (sum % 10)) % 10;
    }

    // IIsinValidator implementation
    ValidationResult IIsinValidator.Validate(string? isin) => Validate(isin);
    IsinDetails? IIsinValidator.Parse(string? isin) => Parse(isin);
}
