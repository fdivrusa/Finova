using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Switzerland.Validators;

public partial class SwitzerlandVatValidator : IVatValidator
{
    // CHE-123.456.789 or 123456789 (UID)
    [GeneratedRegex(@"^\d{9}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "CHE";
    private const string AltPrefix = "CH";

    private static readonly int[] Weights = { 5, 4, 3, 2, 7, 6, 5, 4 };

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

        var cleaned = vat.Trim().ToUpperInvariant()
            .Replace(".", "")
            .Replace("-", "")
            .Replace(" ", "");

        if (cleaned.StartsWith(VatPrefix))
        {
            cleaned = cleaned[3..];
        }
        else if (cleaned.StartsWith(AltPrefix))
        {
            cleaned = cleaned[2..];
        }

        if (!VatRegex().IsMatch(cleaned))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid Switzerland VAT format.");
        }

        // Checksum Validation (Mod 11)
        int sum = ChecksumHelper.CalculateWeightedSum(cleaned.Substring(0, 8), Weights);
        int remainder = sum % 11;
        int checkDigit = 11 - remainder;

        if (checkDigit == 11) checkDigit = 0;
        if (checkDigit == 10)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid Switzerland VAT checksum (Result 10).");
        }

        int lastDigit = cleaned[8] - '0';
        if (checkDigit != lastDigit)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid Switzerland VAT checksum.");
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

        var cleaned = vat!.Trim().ToUpperInvariant()
             .Replace(".", "")
             .Replace("-", "")
             .Replace(" ", "");

        if (cleaned.StartsWith(VatPrefix))
        {
            cleaned = cleaned[3..];
        }
        else if (cleaned.StartsWith(AltPrefix))
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