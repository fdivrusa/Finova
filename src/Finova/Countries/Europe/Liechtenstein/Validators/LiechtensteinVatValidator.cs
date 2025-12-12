using Finova.Core.Common;
using Finova.Core.Vat;

using Finova.Countries.Europe.Switzerland.Validators;

namespace Finova.Countries.Europe.Liechtenstein.Validators;

/// <summary>
/// Validator for Liechtenstein VAT numbers.
/// Liechtenstein uses Swiss VAT numbers (UID/MWST).
/// Format: CHE-123.456.789 (or without dashes/dots).
/// </summary>
public partial class LiechtensteinVatValidator : IVatValidator
{
    private const string SwissPrefix = "CHE";
    public string CountryCode => "LI";

    ValidationResult IValidator<VatDetails>.Validate(string? instance) => Validate(instance);

    public VatDetails? Parse(string? vat) => GetVatDetails(vat);

    public static ValidationResult Validate(string? vat)
    {
        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "VAT number cannot be empty.");
        }

        return SwitzerlandVatValidator.Validate(vat);
    }

    public static VatDetails? GetVatDetails(string? vat)
    {
        var result = Validate(vat);
        if (!result.IsValid)
        {
            return null;
        }

        var cleaned = VatSanitizer.Sanitize(vat)!.Trim().ToUpperInvariant();

        if (cleaned.StartsWith(SwissPrefix))
        {
            cleaned = cleaned[SwissPrefix.Length..];
        }
        else if (cleaned.StartsWith("LI"))
        {
            cleaned = cleaned[2..];
        }

        return new VatDetails
        {
            CountryCode = "LI",
            VatNumber = cleaned,
            IsValid = true
        };
    }
}