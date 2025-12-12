using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Norway.Validators;

public partial class NorwayVatValidator : IVatValidator
{
    [GeneratedRegex(@"^(NO)?\d{9}(MVA)?$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "NO";

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
        if (cleaned.EndsWith("MVA"))
        {
            cleaned = cleaned.Substring(0, cleaned.Length - 3);
        }

        if (cleaned.Length != 9 || !long.TryParse(cleaned, out _))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid Norway VAT format.");
        }

        // Checksum Validation (Mod 11)
        // Weights: 3, 2, 7, 6, 5, 4, 3, 2
        int[] weights = { 3, 2, 7, 6, 5, 4, 3, 2 };

        int sum = ChecksumHelper.CalculateWeightedSum(cleaned.Substring(0, 8), weights);

        int remainder = sum % 11;
        int checkDigit = 11 - remainder;
        if (checkDigit == 11) checkDigit = 0;

        if (checkDigit == 10)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid Norway VAT checksum (Check digit 10).");
        }

        int lastDigit = cleaned[8] - '0';
        if (checkDigit != lastDigit)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid Norway VAT checksum.");
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
        if (cleaned.EndsWith("MVA"))
        {
            cleaned = cleaned.Substring(0, cleaned.Length - 3);
        }

        return new VatDetails
        {
            CountryCode = VatPrefix,
            VatNumber = cleaned,
            IsValid = true
        };
    }
}
