using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.UnitedKingdom.Validators;

public partial class UnitedKingdomVatValidator : IVatValidator
{
    [GeneratedRegex(@"^GB(\d{9}|\d{12}|GD\d{3}|HA\d{3})$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "GB";

    public string CountryCode => VatPrefix;

    ValidationResult IValidator<VatDetails>.Validate(string? instance) => Validate(instance);

    public VatDetails? Parse(string? vat) => GetVatDetails(vat);

    public static ValidationResult Validate(string? vat)
    {
        vat = VatSanitizer.Sanitize(vat);

        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var cleaned = vat.Trim().ToUpperInvariant();
        if (cleaned.StartsWith(VatPrefix))
        {
            cleaned = cleaned[2..];
        }

        // Basic format check
        if (cleaned.StartsWith("GD") || cleaned.StartsWith("HA"))
        {
            // Government departments / Health authorities - 5 chars total
            if (cleaned.Length != 5) return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, string.Format(ValidationMessages.InvalidVatFormat, "UK"));
            return ValidationResult.Success(); // No checksum for these
        }

        if (cleaned.Length != 9 && cleaned.Length != 12)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLength);
        }

        if (!long.TryParse(cleaned, out _))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
        }

        // Checksum Validation (Weighted Mod 97)
        // Only for first 9 digits (even if 12 digits, first 9 are the block)
        string block = cleaned.Substring(0, 9);

        // Weights: 8, 7, 6, 5, 4, 3, 2
        int[] weights = { 8, 7, 6, 5, 4, 3, 2 };

        int sum = ChecksumHelper.CalculateWeightedSum(block.Substring(0, 7), weights);

        int checkDigits = int.Parse(block.Substring(7, 2));
        long totalSum = sum + checkDigits;

        if (totalSum % 97 == 0) return ValidationResult.Success();

        // UK has a secondary algorithm (add 55 to sum)
        if ((totalSum + 55) % 97 == 0) return ValidationResult.Success();

        // If both fail
        return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, string.Format(ValidationMessages.InvalidVatChecksum, "UK"));
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
