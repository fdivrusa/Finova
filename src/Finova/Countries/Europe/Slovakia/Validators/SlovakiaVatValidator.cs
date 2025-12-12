using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Slovakia.Validators;

public partial class SlovakiaVatValidator : IVatValidator
{
    [GeneratedRegex(@"^\d{10}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "SK";

    public string CountryCode => VatPrefix;

    ValidationResult IValidator<VatDetails>.Validate(string? instance) => Validate(instance);

    public VatDetails? Parse(string? vat) => GetVatDetails(vat);

    public static ValidationResult Validate(string? vat)
    {
        vat = VatSanitizer.Sanitize(vat);

        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "VAT number cannot be empty.");
        }

        var cleaned = vat.Trim().ToUpperInvariant();
        if (cleaned.StartsWith(VatPrefix))
        {
            cleaned = cleaned[2..];
        }

        if (!VatRegex().IsMatch(cleaned))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid Slovakia VAT format.");
        }

        // Checksum Validation (Divisible by 11)
        if (!ChecksumHelper.IsDivisibleBy(cleaned, 11))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid Slovakia VAT checksum (Must be divisible by 11).");
        }

        return ValidationResult.Success();
    }

    public static VatDetails? GetVatDetails(string? vat)
    {
        vat = VatSanitizer.Sanitize(vat);

        if (!Validate(vat).IsValid)
        {
            return null;
        }

        var cleaned = vat!.Trim().ToUpperInvariant();
        if (cleaned.StartsWith(VatPrefix))
        {
            cleaned = cleaned[2..];
        }

        return new VatDetails
        {
            CountryCode = VatPrefix,
            VatNumber = cleaned,
            IsValid = true
        };
    }
}
