using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Poland.Validators;

public partial class PolandVatValidator : IVatValidator
{
    [GeneratedRegex(@"^\d{10}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "PL";

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
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid Poland VAT format.");
        }

        int[] weights = { 6, 5, 7, 2, 3, 4, 5, 6, 7 };

        int sum = ChecksumHelper.CalculateWeightedSum(cleaned.Substring(0, 9), weights);

        int remainder = sum % 11;
        if (remainder == 10)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid checksum (Remainder 10).");
        }

        int checkDigit = cleaned[9] - '0';
        if (remainder != checkDigit)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid checksum.");
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
