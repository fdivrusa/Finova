using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.NorthAmerica.Canada.Validators;

/// <summary>
/// Validates Canadian Goods and Services Tax (GST) / Harmonized Sales Tax (HST) number.
/// The GST/HST number consists of the 9-digit Business Number (BN) followed by "RT"
/// and a 4-digit program account number (e.g., 123456789RT0001).
/// </summary>
public class CanadaGstValidator : IVatValidator
{
    private const string CountryCodePrefix = "CA";

    /// <inheritdoc/>
    public string CountryCode => CountryCodePrefix;

    /// <inheritdoc/>
    ValidationResult IValidator<VatDetails>.Validate(string? instance) => Validate(instance);

    /// <inheritdoc/>
    public VatDetails? Parse(string? vat) => GetVatDetails(vat);

    /// <summary>
    /// Validates a Canadian GST/HST number.
    /// </summary>
    /// <param name="gst">The GST/HST number string (9-15 characters).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult Validate(string? gst)
    {
        if (string.IsNullOrWhiteSpace(gst))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = gst.Trim().ToUpperInvariant().Replace(" ", "").Replace("-", "");

        // Remove CA prefix if present
        if (clean.StartsWith("CA"))
        {
            clean = clean[2..];
        }

        // Full GST number format: 123456789RT0001 (15 characters)
        // BN root only: 123456789 (9 characters)

        string bnRoot;

        if (clean.Length == 15)
        {
            // Full format: BN + RT + account
            bnRoot = clean[..9];

            // Check for RT suffix
            if (clean.Substring(9, 2) != "RT")
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat,
                    "GST/HST number must have 'RT' after the 9-digit BN.");
            }

            // Check account number (4 digits)
            string account = clean.Substring(11, 4);
            if (!account.All(char.IsDigit))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat,
                    "GST/HST account number must be 4 digits.");
            }
        }
        else if (clean.Length == 9)
        {
            // BN root only
            bnRoot = clean;
        }
        else if (clean.Length > 9 && clean.Contains("RT"))
        {
            // Partial format, extract BN
            int rtIndex = clean.IndexOf("RT", StringComparison.Ordinal);
            bnRoot = clean[..rtIndex];
        }
        else
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength,
                "GST/HST number must be 9 digits (BN) or 15 characters (full format).");
        }

        // Validate the BN root using the existing validator
        return CanadaBusinessNumberValidator.ValidateBn(bnRoot);
    }

    /// <summary>
    /// Gets details of a validated Canadian GST/HST number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? gst)
    {
        if (!Validate(gst).IsValid)
        {
            return null;
        }

        var clean = gst!.Trim().ToUpperInvariant().Replace(" ", "").Replace("-", "");

        // Remove CA prefix if present
        if (clean.StartsWith("CA"))
        {
            clean = clean[2..];
        }

        return new VatDetails
        {
            VatNumber = clean,
            CountryCode = CountryCodePrefix,
            IsValid = true,
            IdentifierKind = "GST/HST",
            IsEuVat = false,
            IsViesEligible = false,
            Notes = "Canadian Goods and Services Tax / Harmonized Sales Tax"
        };
    }
}
