using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Greece.Validators;

public partial class GreeceVatValidator : IVatValidator
{
    [GeneratedRegex(@"^EL\d{9}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "EL"; // Greece uses EL for VAT, GR for country code usually.

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
        if (cleaned.StartsWith(VatPrefix) || cleaned.StartsWith("GR"))
        {
            cleaned = cleaned[2..];
        }

        if (cleaned.Length != 9 || !long.TryParse(cleaned, out _))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid Greece VAT format.");
        }

        // Checksum Validation (Powers of 2 Mod 11)
        // Weights: 256, 128, 64, 32, 16, 8, 4, 2
        int[] weights = { 256, 128, 64, 32, 16, 8, 4, 2 };

        int sum = ChecksumHelper.CalculateWeightedSum(cleaned.Substring(0, 8), weights);

        int remainder = sum % 11;
        int checkDigit = remainder % 10; // If remainder is 10, check digit is 0.

        int lastDigit = cleaned[8] - '0';
        if (checkDigit != lastDigit)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid Greece VAT checksum.");
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
        if (cleaned.StartsWith(VatPrefix) || cleaned.StartsWith("GR"))
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
