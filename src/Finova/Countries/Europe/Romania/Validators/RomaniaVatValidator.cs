using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Romania.Validators;

public partial class RomaniaVatValidator : IVatValidator
{
    [GeneratedRegex(@"^\d{2,10}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "RO";
    // Weights for Romanian CIF (applied to the first 9 digits, padded with 0 if needed)
    // 7, 5, 3, 2, 1, 7, 5, 3, 2
    private static readonly int[] Weights = { 7, 5, 3, 2, 1, 7, 5, 3, 2 };

    public string CountryCode => VatPrefix;

    ValidationResult IValidator<VatDetails>.Validate(string? instance) => Validate(instance);

    public VatDetails? Parse(string? vat) => GetVatDetails(vat);

    public static ValidationResult Validate(string? vat)
    {
        vat = VatSanitizer.Sanitize(vat);

        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "Empty.");
        }

        var cleaned = vat.Trim().ToUpperInvariant();
        if (cleaned.StartsWith(VatPrefix))
        {
            cleaned = cleaned[2..];
        }

        if (!VatRegex().IsMatch(cleaned))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid Romania VAT format.");
        }

        string padded = cleaned.PadLeft(10, '0');

        string dataPart = padded[..9];
        int checkDigit = padded[9] - '0';

        int sum = ChecksumHelper.CalculateWeightedSum(dataPart, Weights);

        int calculated = sum * 10 % 11;
        if (calculated == 10)
        {
            calculated = 0;
        }

        if (calculated != checkDigit)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid Romania VAT checksum.");
        }

        return ValidationResult.Success();
    }

    public static VatDetails? GetVatDetails(string? vat)
    {
        var result = Validate(vat);
        if (!result.IsValid)
        {
            return null;
        }
        var cleaned = VatSanitizer.Sanitize(vat)!.Trim().ToUpperInvariant();
        if (cleaned.StartsWith(VatPrefix))
        {
            cleaned = cleaned[2..];
        }
        return new VatDetails { CountryCode = VatPrefix, VatNumber = cleaned, IsValid = true };
    }
}