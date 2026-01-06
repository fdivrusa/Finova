using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Asia.Singapore.Validators;

/// <summary>
/// Validates Singapore Goods and Services Tax (GST) registration number.
/// In Singapore, GST-registered businesses use their UEN (Unique Entity Number)
/// followed by a suffix for GST identification.
/// Format: UEN + optional GST suffix (e.g., "M90312345A" or "201234567K" or "201234567K-GST")
/// </summary>
public class SingaporeGstValidator : IVatValidator
{
    private const string CountryCodePrefix = "SG";

    /// <inheritdoc/>
    public string CountryCode => CountryCodePrefix;

    /// <inheritdoc/>
    ValidationResult IValidator<VatDetails>.Validate(string? instance) => Validate(instance);

    /// <inheritdoc/>
    public VatDetails? Parse(string? vat) => GetVatDetails(vat);

    /// <summary>
    /// Validates a Singapore GST registration number.
    /// </summary>
    /// <param name="gst">The GST registration number string.</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult Validate(string? gst)
    {
        if (string.IsNullOrWhiteSpace(gst))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = gst.Trim().ToUpperInvariant()
                       .Replace(" ", "")
                       .Replace("-", "");

        // Remove common GST suffixes if present
        if (clean.EndsWith("GST"))
        {
            clean = clean[..^3];
        }

        // Remove SG prefix if present
        if (clean.StartsWith("SG"))
        {
            clean = clean[2..];
        }

        // Use the existing UEN validator since GST registration uses UEN
        return SingaporeUenValidator.ValidateStatic(clean);
    }

    /// <summary>
    /// Gets details of a validated Singapore GST number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? gst)
    {
        if (!Validate(gst).IsValid)
        {
            return null;
        }

        var clean = gst!.Trim().ToUpperInvariant()
                        .Replace(" ", "")
                        .Replace("-", "");

        // Remove common GST suffixes if present
        if (clean.EndsWith("GST"))
        {
            clean = clean[..^3];
        }

        // Remove SG prefix if present
        if (clean.StartsWith("SG"))
        {
            clean = clean[2..];
        }

        return new VatDetails
        {
            VatNumber = clean,
            CountryCode = CountryCodePrefix,
            IsValid = true,
            IdentifierKind = "GST",
            IsEuVat = false,
            IsViesEligible = false,
            Notes = "Singapore Goods and Services Tax (based on UEN)"
        };
    }
}
