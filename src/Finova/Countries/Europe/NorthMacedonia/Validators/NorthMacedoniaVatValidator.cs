using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.NorthMacedonia.Validators;

public partial class NorthMacedoniaVatValidator : IVatValidator
{
    [GeneratedRegex(@"^\d{13}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "MK";
    private static readonly int[] Weights = { 7, 6, 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };

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
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid North Macedonia VAT format.");
        }

        int sum = ChecksumHelper.CalculateWeightedSum(cleaned[..12], Weights);
        int remainder = sum % 11;
        int checkDigit = 11 - remainder;

        if (checkDigit == 10)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid North Macedonia VAT checksum (Check digit 10).");
        }
        if (checkDigit == 11) checkDigit = 0;

        if (checkDigit != (cleaned[12] - '0'))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid North Macedonia VAT checksum.");
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