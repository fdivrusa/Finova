using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Estonia.Validators;

public partial class EstoniaVatValidator : IVatValidator
{
    [GeneratedRegex(@"^\d{9}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "EE";

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
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid Estonia VAT format.");
        }

        int[] weights = { 3, 7, 1, 3, 7, 1, 3, 7 };

        int sum = ChecksumHelper.CalculateWeightedSum(cleaned.Substring(0, 8), weights);

        int checkDigit = 10 - (sum % 10);
        if (checkDigit == 10) checkDigit = 0;

        int lastDigit = cleaned[8] - '0';
        if (checkDigit != lastDigit)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid Estonia VAT checksum.");
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
