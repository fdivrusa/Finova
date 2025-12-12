using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Lithuania.Validators;

public partial class LithuaniaVatValidator : IVatValidator
{
    [GeneratedRegex(@"^(\d{9}|\d{12})$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "LT";

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
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid Lithuania VAT format.");
        }

        if (cleaned.Length == 9)
        {
            int[] weights1 = { 1, 2, 3, 4, 5, 6, 7, 8 };
            int sum = ChecksumHelper.CalculateWeightedSum(cleaned.Substring(0, 8), weights1);

            int remainder = sum % 11;
            if (remainder == 10)
            {
                int[] weights2 = { 3, 4, 5, 6, 7, 8, 9, 1 };
                sum = ChecksumHelper.CalculateWeightedSum(cleaned.Substring(0, 8), weights2);
                remainder = sum % 11;
                if (remainder == 10) remainder = 0;
            }

            int checkDigit = cleaned[8] - '0';
            if (remainder != checkDigit)
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid Lithuania VAT checksum.");
            }
        }
        else if (cleaned.Length == 12)
        {
            int[] weights1 = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2 };
            int sum = ChecksumHelper.CalculateWeightedSum(cleaned.Substring(0, 11), weights1);

            int remainder = sum % 11;
            if (remainder == 10)
            {
                int[] weights2 = { 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4 };
                sum = ChecksumHelper.CalculateWeightedSum(cleaned.Substring(0, 11), weights2);
                remainder = sum % 11;
                if (remainder == 10) remainder = 0;
            }

            int checkDigit = cleaned[11] - '0';
            if (remainder != checkDigit)
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid Lithuania VAT checksum.");
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
