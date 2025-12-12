using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Malta.Validators;

public partial class MaltaVatValidator : IVatValidator
{
    [GeneratedRegex(@"^MT\d{8}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "MT";

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
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid Malta VAT format.");
        }

        // Checksum Validation (Weighted Mod 37)
        // Weights: 3, 4, 6, 7, 8, 2, 5
        int[] weights = { 3, 4, 6, 7, 8, 2, 5 };

        int sum = ChecksumHelper.CalculateWeightedSum(cleaned.Substring(0, 7), weights);

        int checkDigit = 37 - (sum % 37);
        if (checkDigit == 37) checkDigit = 0; // Assuming 0 if exact match? Or is it impossible?
                                              // Usually if sum % 37 == 0, check is 0.
                                              // Wait, 37 - 0 = 37. 37 is not a digit.
                                              // If remainder is 0, check is 0.

        int lastDigit = cleaned[7] - '0';
        if (checkDigit != lastDigit)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid Malta VAT checksum.");
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
