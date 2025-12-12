using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Luxembourg.Validators;

public partial class LuxembourgVatValidator : IVatValidator
{
    [GeneratedRegex(@"^LU\d{8}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "LU";

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
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid Luxembourg VAT format.");
        }

        // Checksum Validation (Mod 89)
        // First 6 digits % 89 == Last 2 digits
        int firstPart = int.Parse(cleaned.Substring(0, 6));
        int checkDigits = int.Parse(cleaned.Substring(6, 2));

        if (firstPart % 89 != checkDigits)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid Luxembourg VAT checksum.");
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
