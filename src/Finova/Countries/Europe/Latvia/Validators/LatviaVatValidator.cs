using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Latvia.Validators;

public partial class LatviaVatValidator : IVatValidator
{
    [GeneratedRegex(@"^\d{11}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "LV";

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

        if (!VatRegex().IsMatch(cleaned))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidLatviaVatFormat);
        }

        int[] weights = { 9, 1, 4, 8, 3, 10, 2, 5, 7, 6 };

        int sum = ChecksumHelper.CalculateWeightedSum(cleaned.Substring(0, 10), weights);

        int remainder = sum % 11;
        int checkDigit = 3 - remainder;
        if (checkDigit < -1)
        {
            checkDigit += 11;
        }

        if (checkDigit == -1)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidLatviaVatChecksumResultMinusOne);
        }

        int lastDigit = cleaned[10] - '0';
        if (checkDigit != lastDigit)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidLatviaVatChecksum);
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
