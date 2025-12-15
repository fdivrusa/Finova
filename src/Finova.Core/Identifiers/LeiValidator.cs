using System.Numerics;
using System.Text.RegularExpressions;
using Finova.Core.Common;

namespace Finova.Core.Identifiers;

/// <summary>
/// Validator for Legal Entity Identifier (LEI) based on ISO 17442.
/// </summary>
public partial class LeiValidator : IValidator<string>
{
    [GeneratedRegex(@"^[A-Z0-9]{20}$")]
    private static partial Regex LeiRegex();

    /// <summary>
    /// Validates a Legal Entity Identifier (LEI).
    /// </summary>
    /// <param name="instance">The LEI string to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public ValidationResult Validate(string? instance)
    {
        if (string.IsNullOrWhiteSpace(instance))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "LEI cannot be empty.");
        }

        var normalized = Parse(instance);

        if (normalized == null || normalized.Length != 20)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "LEI must be exactly 20 characters.");
        }

        if (!LeiRegex().IsMatch(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "LEI must contain only alphanumeric characters.");
        }

        // ISO 7064 Mod 97-10 Validation
        var sb = new System.Text.StringBuilder(normalized.Length * 2);
        foreach (char c in normalized)
        {
            if (char.IsDigit(c))
            {
                sb.Append(c);
            }
            else
            {
                // A=10, B=11, ... Z=35
                sb.Append(c - 'A' + 10);
            }
        }

        if (!BigInteger.TryParse(sb.ToString(), out BigInteger bigInt))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Failed to parse LEI for checksum calculation.");
        }

        if (bigInt % 97 != 1)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid LEI checksum.");
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Normalizes the LEI string (trims whitespace and converts to uppercase).
    /// </summary>
    /// <param name="instance">The LEI string.</param>
    /// <returns>The normalized LEI string.</returns>
    public string? Parse(string? instance)
    {
        if (string.IsNullOrWhiteSpace(instance))
        {
            return null;
        }
        return instance.Trim().ToUpperInvariant();
    }
}
