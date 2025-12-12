using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Montenegro.Validators;

/// <summary>
/// Validator for Montenegro VAT numbers.
/// Format : ME + 7 digits
/// </summary>
public partial class MontenegroVatValidator : IVatValidator
{
    [GeneratedRegex(@"^\d{8}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "ME";
    private static readonly int[] Weights = { 7, 6, 5, 4, 3, 2, 7 };

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
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid Montenegro VAT format.");
        }

        int sum = ChecksumHelper.CalculateWeightedSum(cleaned[..7], Weights);
        int remainder = sum % 11;
        int checkDigit;

        if (remainder == 10)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid Montenegro VAT checksum (Remainder 10).");
        }

        checkDigit = 11 - remainder;
        if (checkDigit == 11) checkDigit = 0;

        if (checkDigit != (cleaned[7] - '0'))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid Montenegro VAT checksum.");
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