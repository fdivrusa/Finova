using System.Text.RegularExpressions;
using Finova.Core.Common;

namespace Finova.Core.Identifiers;

/// <summary>
/// Details parsed from a SEDOL (Stock Exchange Daily Official List).
/// </summary>
public class SedolDetails
{
    /// <summary>
    /// The full normalized SEDOL code (7 characters).
    /// </summary>
    public string Sedol { get; init; } = string.Empty;

    /// <summary>
    /// The base identifier (6 characters).
    /// </summary>
    public string BaseCode { get; init; } = string.Empty;

    /// <summary>
    /// The check digit (1 character).
    /// </summary>
    public char CheckDigit { get; init; }

    /// <summary>
    /// Whether the SEDOL is valid.
    /// </summary>
    public bool IsValid { get; init; }
}

/// <summary>
/// Interface for SEDOL validation.
/// </summary>
public interface ISedolValidator
{
    /// <summary>
    /// Validates a SEDOL (Stock Exchange Daily Official List).
    /// </summary>
    ValidationResult Validate(string? sedol);

    /// <summary>
    /// Parses a SEDOL and returns detailed information.
    /// </summary>
    SedolDetails? Parse(string? sedol);
}

/// <summary>
/// Validator for SEDOL (Stock Exchange Daily Official List).
/// A SEDOL uniquely identifies UK and Irish securities.
/// </summary>
/// <remarks>
/// Format: 6-character base code + 1 check digit = 7 characters total.
/// The check digit is calculated using weighted sum modulo 10.
/// Vowels (A, E, I, O, U) are not allowed in SEDOL codes.
/// </remarks>
/// <example>
/// <code>
/// // Validate a SEDOL
/// var result = SedolValidator.Validate("0263494"); // BAE Systems
/// if (result.IsValid)
/// {
///     Console.WriteLine("Valid SEDOL");
/// }
/// 
/// // Parse a SEDOL
/// var details = SedolValidator.Parse("0263494");
/// Console.WriteLine($"Base: {details?.BaseCode}"); // 026349
/// </code>
/// </example>
public partial class SedolValidator : ISedolValidator
{
    // SEDOL characters: 0-9 and B, C, D, F, G, H, J, K, L, M, N, P, Q, R, S, T, V, W, X, Y, Z
    // Vowels (A, E, I, O, U) are excluded
    [GeneratedRegex(@"^[0-9BCDFGHJKLMNPQRSTVWXYZ]{6}[0-9]$")]
    private static partial Regex SedolRegex();

    // Weights for SEDOL checksum calculation
    private static readonly int[] Weights = [1, 3, 1, 7, 3, 9, 1];

    /// <summary>
    /// Validates a SEDOL identifier.
    /// </summary>
    /// <param name="sedol">The SEDOL string to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public static ValidationResult Validate(string? sedol)
    {
        if (string.IsNullOrWhiteSpace(sedol))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = Normalize(sedol);

        if (normalized.Length != 7)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "SEDOL must be exactly 7 characters.");
        }

        if (!SedolRegex().IsMatch(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "SEDOL contains invalid characters. Vowels (A, E, I, O, U) are not allowed.");
        }

        // Validate checksum
        if (!ValidateChecksum(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid SEDOL checksum.");
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Parses a SEDOL and returns detailed information.
    /// </summary>
    /// <param name="sedol">The SEDOL string to parse.</param>
    /// <returns>A <see cref="SedolDetails"/> object if valid, otherwise null.</returns>
    public static SedolDetails? Parse(string? sedol)
    {
        if (string.IsNullOrWhiteSpace(sedol))
        {
            return null;
        }

        var normalized = Normalize(sedol);

        if (normalized.Length != 7)
        {
            return null;
        }

        var isValid = SedolRegex().IsMatch(normalized) && ValidateChecksum(normalized);

        return new SedolDetails
        {
            Sedol = normalized,
            BaseCode = normalized[..6],
            CheckDigit = normalized[6],
            IsValid = isValid
        };
    }

    /// <summary>
    /// Calculates the check digit for a SEDOL base (6 characters without check digit).
    /// </summary>
    /// <param name="sedolBase">The first 6 characters of the SEDOL.</param>
    /// <returns>The calculated check digit (0-9).</returns>
    public static char CalculateCheckDigit(string sedolBase)
    {
        if (string.IsNullOrWhiteSpace(sedolBase) || sedolBase.Length != 6)
        {
            throw new ArgumentException("SEDOL base must be exactly 6 characters.", nameof(sedolBase));
        }

        var normalized = sedolBase.ToUpperInvariant();
        int sum = 0;

        for (int i = 0; i < 6; i++)
        {
            char c = normalized[i];
            int value = GetCharacterValue(c);
            if (value < 0)
            {
                throw new ArgumentException($"Invalid character '{c}' in SEDOL base.", nameof(sedolBase));
            }
            sum += value * Weights[i];
        }

        return (char)('0' + (10 - (sum % 10)) % 10);
    }

    /// <summary>
    /// Generates a valid SEDOL from a base code.
    /// </summary>
    /// <param name="sedolBase">The 6-character SEDOL base.</param>
    /// <returns>A valid 7-character SEDOL.</returns>
    public static string Generate(string sedolBase)
    {
        if (string.IsNullOrWhiteSpace(sedolBase) || sedolBase.Length != 6)
        {
            throw new ArgumentException("SEDOL base must be exactly 6 characters.", nameof(sedolBase));
        }

        var normalized = sedolBase.ToUpperInvariant();
        var checkDigit = CalculateCheckDigit(normalized);
        return normalized + checkDigit;
    }

    private static string Normalize(string sedol)
    {
        return sedol.Trim().ToUpperInvariant().Replace(" ", "").Replace("-", "");
    }

    private static bool ValidateChecksum(string sedol)
    {
        int sum = 0;

        for (int i = 0; i < 7; i++)
        {
            char c = sedol[i];
            int value = GetCharacterValue(c);
            if (value < 0)
            {
                return false;
            }
            sum += value * Weights[i];
        }

        return sum % 10 == 0;
    }

    private static int GetCharacterValue(char c)
    {
        if (char.IsDigit(c))
        {
            return c - '0';
        }

        // Vowels are not allowed
        if (c is 'A' or 'E' or 'I' or 'O' or 'U')
        {
            return -1;
        }

        if (c >= 'B' && c <= 'Z')
        {
            return c - 'A' + 10;
        }

        return -1;
    }

    // ISedolValidator implementation
    ValidationResult ISedolValidator.Validate(string? sedol) => Validate(sedol);
    SedolDetails? ISedolValidator.Parse(string? sedol) => Parse(sedol);
}
