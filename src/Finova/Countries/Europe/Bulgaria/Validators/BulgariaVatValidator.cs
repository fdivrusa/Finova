using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Bulgaria.Validators;

/// <summary>
/// Validator for Bulgarian VAT numbers.
/// Format: BG + 9 or 10 digits depending on the type of VAT number.
/// </summary>
public partial class BulgariaVatValidator : IVatValidator
{
    [GeneratedRegex(@"^\d{9,10}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "BG";

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
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid Bulgaria VAT format.");
        }

        bool isValidChecksum;
        if (cleaned.Length == 9)
        {
            isValidChecksum = Validate9DigitVat(cleaned);
        }
        else
        {
            // 10 digits: Try EGN (Physical) first, then PNF (Foreigner)
            isValidChecksum = Validate10DigitVat(cleaned);
        }

        if (!isValidChecksum)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid Bulgaria VAT checksum.");
        }

        return ValidationResult.Success();
    }

    private static bool Validate9DigitVat(string vat)
    {
        int[] weights1 = { 1, 2, 3, 4, 5, 6, 7, 8 };
        int sum = 0;

        for (int i = 0; i < 8; i++)
        {
            sum += (vat[i] - '0') * weights1[i];
        }

        int remainder = sum % 11;

        if (remainder != 10)
        {
            return remainder == (vat[8] - '0');
        }

        int[] weights2 = { 3, 4, 5, 6, 7, 8, 9, 10 };
        sum = 0;
        for (int i = 0; i < 8; i++)
        {
            sum += (vat[i] - '0') * weights2[i];
        }

        remainder = sum % 11;
        if (remainder == 10) remainder = 0;

        return remainder == (vat[8] - '0');
    }

    private static bool Validate10DigitVat(string vat)
    {
        int[] weightsEgn = { 2, 4, 8, 5, 10, 9, 7, 3, 6 };
        int sum = 0;
        for (int i = 0; i < 9; i++)
        {
            sum += (vat[i] - '0') * weightsEgn[i];
        }
        int remainder = sum % 11;
        if (remainder == 10) remainder = 0;

        if (remainder == (vat[9] - '0')) return true;

        int[] weightsPnf = { 21, 19, 17, 13, 11, 9, 7, 3, 1 };
        sum = 0;
        for (int i = 0; i < 9; i++)
        {
            sum += (vat[i] - '0') * weightsPnf[i];
        }
        remainder = sum % 10;

        return remainder == (vat[9] - '0');
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