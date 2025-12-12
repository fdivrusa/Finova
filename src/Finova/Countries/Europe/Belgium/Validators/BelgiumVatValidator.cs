using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Belgium.Validators;

/// <summary>
/// Validator for Belgian VAT numbers (BTW/TVA).
/// Format: BE0xxx.xxx.xxx or BE0xxxxxxxxx (10 digits total).
/// Note: Belgian VAT numbers are essentially the same as Enterprise Numbers (KBO/BCE) with "BE" prefix.
/// </summary>
public partial class BelgiumVatValidator : IVatValidator
{
    [GeneratedRegex(@"[^\d]")]
    private static partial Regex DigitsOnlyRegex();

    private const int VatLength = 10;
    private const string VatPrefix = "BE";

    public string CountryCode => VatPrefix;

    ValidationResult IValidator<VatDetails>.Validate(string? instance) => Validate(instance);

    public VatDetails? Parse(string? vat) => GetVatDetails(vat);

    /// <summary>
    /// Validates a Belgian VAT number.
    /// Accepts formats: BE0123.456.789, BE0123456789, 0123.456.789, or 0123456789.
    /// </summary>
    /// <param name="vat">The VAT number to validate</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult Validate(string? vat)
    {
        vat = VatSanitizer.Sanitize(vat);

        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "VAT number cannot be empty.");
        }

        // Remove "BE" prefix if present
        var cleaned = vat.Trim().ToUpperInvariant();
        if (cleaned.StartsWith(VatPrefix))
        {
            cleaned = cleaned[VatPrefix.Length..];
        }

        // Auto-correction for historical 9-digit numbers
        if (cleaned.Length == 9)
        {
            cleaned = "0" + cleaned;
        }

        // Delegate to Enterprise Number validator (VAT = KBO/BCE)
        return BelgiumEnterpriseValidator.Validate(cleaned);
    }

    /// <summary>
    /// Formats a Belgian VAT number in the standard format: BE 0xxx.xxx.xxx.
    /// </summary>
    /// <param name="vat">The VAT number to format</param>
    /// <returns>Formatted VAT number</returns>
    /// <exception cref="ArgumentException">If the VAT number is invalid</exception>
    public static string Format(string? vat)
    {
        if (!Validate(vat).IsValid)
        {
            throw new ArgumentException("Invalid Belgian VAT number", nameof(vat));
        }

        // Remove BE prefix if present
        var cleaned = vat!.Trim().ToUpperInvariant();
        if (cleaned.StartsWith(VatPrefix))
        {
            cleaned = cleaned[VatPrefix.Length..];
        }

        var digits = DigitsOnlyRegex().Replace(cleaned, "");

        // Format as: BE 0xxx.xxx.xxx
        return $"{VatPrefix} {digits[0]}{digits.Substring(1, 3)}.{digits.Substring(4, 3)}.{digits.Substring(7, 3)}";
    }

    /// <summary>
    /// Normalizes a Belgian VAT number by keeping only digits and the BE prefix.
    /// </summary>
    /// <param name="vat">The VAT number to normalize</param>
    /// <returns>Normalized VAT number (BExxxxxxxxxx)</returns>
    public static string Normalize(string? vat)
    {
        if (string.IsNullOrWhiteSpace(vat))
        {
            return string.Empty;
        }

        var cleaned = vat.Trim().ToUpperInvariant();
        var digits = DigitsOnlyRegex().Replace(cleaned, "");

        if (digits.Length == VatLength)
        {
            return $"{VatPrefix}{digits}";
        }

        return string.Empty;
    }

    /// <summary>
    /// Extracts the Enterprise Number (KBO/BCE) from a VAT number.
    /// </summary>
    /// <param name="vat">The VAT number</param>
    /// <returns>The corresponding KBO/BCE number or null if invalid</returns>
    public static string? GetEnterpriseNumber(string? vat)
    {
        if (!Validate(vat).IsValid)
        {
            return null;
        }

        var cleaned = vat!.Trim().ToUpperInvariant();
        if (cleaned.StartsWith(VatPrefix))
        {
            cleaned = cleaned[VatPrefix.Length..];
        }

        return BelgiumEnterpriseValidator.Normalize(cleaned);
    }

    /// <summary>
    /// Parses a Belgian VAT number and returns details.
    /// </summary>
    /// <param name="vat">The VAT number to parse.</param>
    /// <returns>VatDetails object or null if invalid.</returns>
    public static Core.Vat.VatDetails? GetVatDetails(string? vat)
    {
        if (!Validate(vat).IsValid)
        {
            return null;
        }

        return new Core.Vat.VatDetails
        {
            VatNumber = Normalize(vat),
            CountryCode = "BE",
            IsValid = true
        };
    }
}
