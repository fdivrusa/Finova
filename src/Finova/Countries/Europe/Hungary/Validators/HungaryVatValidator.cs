using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Hungary.Validators;

public partial class HungaryVatValidator : IVatValidator
{
    [GeneratedRegex(@"^HU\d{8}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "HU";

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

        if (cleaned.Length != 8 || !long.TryParse(cleaned, out _))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid Hungary VAT format.");
        }

        // Checksum Validation (Weighted Mod 10)
        // Weights: 9, 7, 3, 1, 9, 7, 3
        int[] weights = { 9, 7, 3, 1, 9, 7, 3 };

        int sum = ChecksumHelper.CalculateWeightedSum(cleaned.Substring(0, 7), weights);

        int checkDigit = 10 - (sum % 10);
        if (checkDigit == 10) checkDigit = 0;

        int lastDigit = cleaned[7] - '0';
        if (checkDigit != lastDigit)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid Hungary VAT checksum.");
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
