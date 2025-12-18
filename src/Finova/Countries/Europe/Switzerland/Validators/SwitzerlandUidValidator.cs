using Finova.Core.Common;
using Finova.Core.Identifiers;
using Finova.Countries.Europe.Switzerland.Validators;

namespace Finova.Countries.Europe.Switzerland.Validators;

/// <summary>
/// Validator for Switzerland UID (Unternehmens-Identifikationsnummer).
/// Wraps the existing SwitzerlandVatValidator as the UID format is identical to the VAT format.
/// </summary>
public class SwitzerlandUidValidator : ITaxIdValidator
{
    public string CountryCode => "CH";

    public ValidationResult Validate(string? instance) => ValidateUid(instance);

    public string? Parse(string? instance) => Normalize(instance);

    /// <summary>
    /// Validates a Switzerland UID.
    /// </summary>
    public static ValidationResult ValidateUid(string? number)
    {
        // Reuse the logic from SwitzerlandVatValidator as UID and VAT are the same number.
        return SwitzerlandVatValidator.Validate(number);
    }

    public static string Format(string? number)
    {
        if (!ValidateUid(number).IsValid)
        {
            throw new ArgumentException("Invalid Input", nameof(number));
        }

        var normalized = Normalize(number);
        // Format: CHE-123.456.789
        return $"CHE-{normalized.Substring(0, 3)}.{normalized.Substring(3, 3)}.{normalized.Substring(6, 3)}";
    }

    public static string Normalize(string? number)
    {
        if (string.IsNullOrWhiteSpace(number)) return string.Empty;

        // Reuse normalization logic or implement specific one if needed.
        // SwitzerlandVatValidator sanitizes and removes CHE/CH prefix.
        // We want the raw digits here.

        var normalized = number.Trim().ToUpperInvariant()
            .Replace(".", "")
            .Replace("-", "")
            .Replace(" ", "");

        if (normalized.StartsWith("CHE"))
        {
            normalized = normalized[3..];
        }
        else if (normalized.StartsWith("CH"))
        {
            normalized = normalized[2..];
        }

        return normalized;
    }
}
