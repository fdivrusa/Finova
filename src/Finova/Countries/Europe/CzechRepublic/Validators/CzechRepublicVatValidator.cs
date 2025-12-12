using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.CzechRepublic.Validators;

public partial class CzechRepublicVatValidator : IVatValidator
{
    [GeneratedRegex(@"^CZ\d{8,10}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "CZ";

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

        if (!VatRegex().IsMatch(VatPrefix + cleaned)) // Regex expects CZ prefix
        {
            // Regex check might fail if we stripped CZ. Let's check digits only.
            if (!long.TryParse(cleaned, out _))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid Czech Republic VAT format.");
            }
        }

        if (cleaned.Length < 8 || cleaned.Length > 10)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "Invalid length.");
        }

        // Checksum Validation
        // Standard (Legal Entities): 8 digits. Weighted Mod 11 {8, 7, 6, 5, 4, 3, 2}
        if (cleaned.Length == 8)
        {
            int[] weights = { 8, 7, 6, 5, 4, 3, 2 };
            int sum = ChecksumHelper.CalculateWeightedSum(cleaned.Substring(0, 7), weights);

            int remainder = sum % 11;
            int checkDigit = 11 - remainder;
            if (checkDigit == 10) checkDigit = 0;
            if (checkDigit == 11) checkDigit = 1;

            int lastDigit = cleaned[7] - '0';
            if (checkDigit != lastDigit)
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid Czech Republic VAT checksum.");
            }
        }
        // Special (Individuals): 9 or 10 digits. Divisible by 11.
        else if (cleaned.Length == 9 || cleaned.Length == 10)
        {
            if (!ChecksumHelper.IsDivisibleBy(cleaned, 11))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid Czech Republic VAT checksum (Must be divisible by 11).");
            }
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
