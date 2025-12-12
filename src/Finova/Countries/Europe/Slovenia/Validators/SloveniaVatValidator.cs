using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Slovenia.Validators;

/// <summary>
/// Validator for Slovenia VAT numbers (ID za DDV).
/// Format: SI + 8 digits.
/// Algorithm: Weighted Modulo 11.
/// </summary>
public partial class SloveniaVatValidator : IVatValidator
{
    [GeneratedRegex(@"^SI\d{8}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "SI";
    private static readonly int[] Weights = [8, 7, 6, 5, 4, 3, 2];

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

        if (!VatRegex().IsMatch(VatPrefix + cleaned))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid Slovenia VAT format.");
        }

        int sum = ChecksumHelper.CalculateWeightedSum(cleaned[..7], Weights);
        int remainder = sum % 11;
        int checkDigit = 11 - remainder;

        if (checkDigit == 10)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid Slovenia VAT checksum (Check digit 10 is invalid).");
        }

        if (checkDigit == 11)
        {
            checkDigit = 0;
        }

        int lastDigit = cleaned[7] - '0';
        if (checkDigit != lastDigit)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid Slovenia VAT checksum.");
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