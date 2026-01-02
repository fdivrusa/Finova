using System.Text.RegularExpressions;
using Finova.Core.Common;

namespace Finova.Core.Identifiers;

/// <summary>
/// Details parsed from a CUSIP (Committee on Uniform Securities Identification Procedures).
/// </summary>
public class CusipDetails
{
    /// <summary>
    /// The full normalized CUSIP code (9 characters).
    /// </summary>
    public string Cusip { get; init; } = string.Empty;

    /// <summary>
    /// The issuer number (6 characters).
    /// </summary>
    public string IssuerNumber { get; init; } = string.Empty;

    /// <summary>
    /// The issue number (2 characters).
    /// </summary>
    public string IssueNumber { get; init; } = string.Empty;

    /// <summary>
    /// The check digit (1 character).
    /// </summary>
    public char CheckDigit { get; init; }

    /// <summary>
    /// Whether the CUSIP is valid.
    /// </summary>
    public bool IsValid { get; init; }
}

/// <summary>
/// Interface for CUSIP validation.
/// </summary>
public interface ICusipValidator
{
    /// <summary>
    /// Validates a CUSIP (Committee on Uniform Securities Identification Procedures).
    /// </summary>
    ValidationResult Validate(string? cusip);

    /// <summary>
    /// Parses a CUSIP and returns detailed information.
    /// </summary>
    CusipDetails? Parse(string? cusip);
}

/// <summary>
/// Validator for CUSIP (Committee on Uniform Securities Identification Procedures).
/// A CUSIP uniquely identifies US and Canadian securities.
/// </summary>
/// <remarks>
/// Format: 6-character issuer number + 2-character issue number + 1 check digit = 9 characters total.
/// The check digit is calculated using a modified Luhn algorithm.
/// </remarks>
/// <example>
/// <code>
/// // Validate a CUSIP
/// var result = CusipValidator.Validate("037833100"); // Apple Inc.
/// if (result.IsValid)
/// {
///     Console.WriteLine("Valid CUSIP");
/// }
/// 
/// // Parse a CUSIP
/// var details = CusipValidator.Parse("037833100");
/// Console.WriteLine($"Issuer: {details?.IssuerNumber}"); // 037833
/// Console.WriteLine($"Issue: {details?.IssueNumber}"); // 10
/// </code>
/// </example>
public partial class CusipValidator : ICusipValidator
{
    [GeneratedRegex(@"^[A-Z0-9]{8}[0-9]$")]
    private static partial Regex CusipRegex();

    /// <summary>
    /// Validates a CUSIP identifier.
    /// </summary>
    /// <param name="cusip">The CUSIP string to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public static ValidationResult Validate(string? cusip)
    {
        if (string.IsNullOrWhiteSpace(cusip))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = Normalize(cusip);

        if (normalized.Length != 9)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidCusipLength);
        }

        if (!CusipRegex().IsMatch(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidCusipFormat);
        }

        // Validate checksum using CUSIP algorithm
        if (!ValidateChecksum(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidCusipChecksum);
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Parses a CUSIP and returns detailed information.
    /// </summary>
    /// <param name="cusip">The CUSIP string to parse.</param>
    /// <returns>A <see cref="CusipDetails"/> object if valid, otherwise null.</returns>
    public static CusipDetails? Parse(string? cusip)
    {
        if (string.IsNullOrWhiteSpace(cusip))
        {
            return null;
        }

        var normalized = Normalize(cusip);

        if (normalized.Length != 9)
        {
            return null;
        }

        var isValid = CusipRegex().IsMatch(normalized) && ValidateChecksum(normalized);

        return new CusipDetails
        {
            Cusip = normalized,
            IssuerNumber = normalized[..6],
            IssueNumber = normalized[6..8],
            CheckDigit = normalized[8],
            IsValid = isValid
        };
    }

    /// <summary>
    /// Calculates the check digit for a CUSIP base (8 characters without check digit).
    /// </summary>
    /// <param name="cusipBase">The first 8 characters of the CUSIP.</param>
    /// <returns>The calculated check digit (0-9).</returns>
    public static char CalculateCheckDigit(string cusipBase)
    {
        if (string.IsNullOrWhiteSpace(cusipBase) || cusipBase.Length != 8)
        {
            throw new ArgumentException("CUSIP base must be exactly 8 characters.", nameof(cusipBase));
        }

        var normalized = cusipBase.ToUpperInvariant();
        int sum = 0;

        for (int i = 0; i < 8; i++)
        {
            char c = normalized[i];
            int value;

            if (char.IsDigit(c))
            {
                value = c - '0';
            }
            else if (char.IsLetter(c))
            {
                // A=10, B=11, ..., Z=35
                value = c - 'A' + 10;
            }
            else if (c == '*')
            {
                value = 36;
            }
            else if (c == '@')
            {
                value = 37;
            }
            else if (c == '#')
            {
                value = 38;
            }
            else
            {
                throw new ArgumentException($"Invalid character '{c}' in CUSIP base.", nameof(cusipBase));
            }

            // Double every second digit (0-indexed position 1, 3, 5, 7)
            if (i % 2 == 1)
            {
                value *= 2;
            }

            // Sum the digits
            sum += value / 10 + value % 10;
        }

        return (char)('0' + (10 - (sum % 10)) % 10);
    }

    /// <summary>
    /// Generates a valid CUSIP from issuer and issue numbers.
    /// </summary>
    /// <param name="issuerNumber">The 6-character issuer number.</param>
    /// <param name="issueNumber">The 2-character issue number.</param>
    /// <returns>A valid 9-character CUSIP.</returns>
    public static string Generate(string issuerNumber, string issueNumber)
    {
        if (string.IsNullOrWhiteSpace(issuerNumber) || issuerNumber.Length != 6)
        {
            throw new ArgumentException("Issuer number must be exactly 6 characters.", nameof(issuerNumber));
        }

        if (string.IsNullOrWhiteSpace(issueNumber) || issueNumber.Length != 2)
        {
            throw new ArgumentException("Issue number must be exactly 2 characters.", nameof(issueNumber));
        }

        var cusipBase = (issuerNumber + issueNumber).ToUpperInvariant();
        var checkDigit = CalculateCheckDigit(cusipBase);
        return cusipBase + checkDigit;
    }

    private static string Normalize(string cusip)
    {
        return cusip.Trim().ToUpperInvariant().Replace(" ", "").Replace("-", "");
    }

    private static bool ValidateChecksum(string cusip)
    {
        int sum = 0;

        for (int i = 0; i < 8; i++)
        {
            char c = cusip[i];
            int value;

            if (char.IsDigit(c))
            {
                value = c - '0';
            }
            else if (char.IsLetter(c))
            {
                value = c - 'A' + 10;
            }
            else if (c == '*')
            {
                value = 36;
            }
            else if (c == '@')
            {
                value = 37;
            }
            else if (c == '#')
            {
                value = 38;
            }
            else
            {
                return false;
            }

            if (i % 2 == 1)
            {
                value *= 2;
            }

            sum += value / 10 + value % 10;
        }

        int expectedCheckDigit = (10 - (sum % 10)) % 10;
        int actualCheckDigit = cusip[8] - '0';

        return expectedCheckDigit == actualCheckDigit;
    }

    // ICusipValidator implementation
    ValidationResult ICusipValidator.Validate(string? cusip) => Validate(cusip);
    CusipDetails? ICusipValidator.Parse(string? cusip) => Parse(cusip);
}
