using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Oceania.Australia.Validators;

/// <summary>
/// Validates Australian Goods and Services Tax (GST) registration.
/// In Australia, the ABN (Australian Business Number) is used for GST registration.
/// GST-registered businesses have their ABN with GST registration status.
/// </summary>
public class AustraliaGstValidator : IVatValidator
{
    private const string CountryCodePrefix = "AU";

    /// <inheritdoc/>
    public string CountryCode => CountryCodePrefix;

    /// <inheritdoc/>
    ValidationResult IValidator<VatDetails>.Validate(string? instance) => Validate(instance);

    /// <inheritdoc/>
    public VatDetails? Parse(string? vat) => GetVatDetails(vat);

    /// <summary>
    /// Validates an Australian GST number (which is the ABN).
    /// </summary>
    /// <param name="gst">The GST/ABN string (11 digits).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult Validate(string? gst)
    {
        if (string.IsNullOrWhiteSpace(gst))
        {
            return AustraliaAbnValidator.ValidateAbn(gst);
        }

        var clean = gst.Trim().Replace(" ", "").Replace("-", "");

        // Remove AU prefix if present
        if (clean.StartsWith("AU", StringComparison.OrdinalIgnoreCase))
        {
            clean = clean[2..];
        }

        // Use the existing ABN validator since GST registration uses ABN
        return AustraliaAbnValidator.ValidateAbn(clean);
    }

    /// <summary>
    /// Gets details of a validated Australian GST number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? gst)
    {
        if (!Validate(gst).IsValid)
        {
            return null;
        }

        var clean = gst!.Replace(" ", "").Replace("-", "");

        return new VatDetails
        {
            VatNumber = clean,
            CountryCode = CountryCodePrefix,
            IsValid = true,
            IdentifierKind = "GST",
            IsEuVat = false,
            IsViesEligible = false,
            Notes = "Australian Goods and Services Tax (based on ABN)"
        };
    }
}
