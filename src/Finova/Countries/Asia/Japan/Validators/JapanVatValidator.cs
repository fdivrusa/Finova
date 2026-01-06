using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Asia.Japan.Validators;

/// <summary>
/// Validates Japanese Consumption Tax registration numbers.
/// Japan's consumption tax (消費税, shōhizei) uses the Corporate Number (法人番号)
/// prefixed with 'T' for registered invoice issuers.
/// Format: T + 13 digits (Corporate Number).
/// </summary>
public class JapanVatValidator : IVatValidator
{
    private const string CountryCodePrefix = "JP";

    /// <inheritdoc/>
    public string CountryCode => CountryCodePrefix;

    /// <inheritdoc/>
    ValidationResult IValidator<VatDetails>.Validate(string? instance) => Validate(instance);

    /// <inheritdoc/>
    public VatDetails? Parse(string? vat) => GetVatDetails(vat);

    /// <summary>
    /// Validates a Japanese Consumption Tax registration number.
    /// </summary>
    /// <param name="vat">The registration number (T + 13 digits).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult Validate(string? vat)
    {
        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = vat.Trim().Replace(" ", "").Replace("-", "");

        // Remove JP prefix if present
        if (clean.StartsWith("JP", StringComparison.OrdinalIgnoreCase))
        {
            clean = clean[2..];
        }

        // Format: T + 13-digit corporate number
        if (clean.Length < 1)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "Japanese consumption tax number must start with 'T' followed by 13 digits.");
        }

        // Check for T prefix (required for qualified invoice issuer)
        bool hasPrefix = clean[0] is 'T' or 't';
        var corporateNumber = hasPrefix ? clean[1..] : clean;

        // Validate the corporate number portion
        return JapanCorporateNumberValidator.ValidateStatic(corporateNumber);
    }

    /// <summary>
    /// Gets details of a validated Japanese Consumption Tax number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat)
    {
        if (!Validate(vat).IsValid)
        {
            return null;
        }

        var clean = vat!.Trim().Replace(" ", "").Replace("-", "");

        // Remove JP prefix if present
        if (clean.StartsWith("JP", StringComparison.OrdinalIgnoreCase))
        {
            clean = clean[2..];
        }

        // Ensure T prefix for standardized format
        clean = "T" + (clean[0] is 'T' or 't' ? clean[1..] : clean);

        return new VatDetails
        {
            VatNumber = clean,
            CountryCode = CountryCodePrefix,
            IsValid = true,
            IdentifierKind = "Invoice Registration Number",
            IsEuVat = false,
            IsViesEligible = false,
            Notes = "Japanese qualified invoice issuer registration number (適格請求書発行事業者登録番号)"
        };
    }
}
