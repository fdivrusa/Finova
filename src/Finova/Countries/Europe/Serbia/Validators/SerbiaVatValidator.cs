using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Serbia.Validators;

/// <summary>
/// Validator for Serbia VAT numbers (PIB).
/// Format: RS + 9 digits.
/// Algorithm: ISO 7064 Mod 11, 10.
/// </summary>
public partial class SerbiaVatValidator : IVatValidator
{
    [GeneratedRegex(@"^RS\d{9}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "RS";

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

        if (!VatRegex().IsMatch(VatPrefix + cleaned))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid Serbia VAT format.");
        }

        if (!ChecksumHelper.ValidateISO7064Mod11_10(cleaned))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid Serbia VAT checksum.");
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