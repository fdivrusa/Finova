using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Turkey.Validators;

public partial class TurkeyVatValidator : IVatValidator
{
    [GeneratedRegex(@"^\d{10}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "TR";

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
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid Turkey VAT format.");
        }

        // Checksum Validation (Mod 10)
        // Algorithm:
        // 1. Process first 9 digits.
        // 2. For each digit i (1-indexed):
        //    d = digit at i
        //    sum = (d + (10 - i)) % 10
        //    if sum == 9, result = 9
        //    else result = (sum * 2^(10-i)) % 9
        //    
        //    Wait, the algorithm is:
        //    For i from 1 to 9:
        //      C1 = (d_i + (10 - i)) % 10
        //      if C1 == 9: C2 = 9
        //      else: C2 = (C1 * 2^(10-i)) % 9
        //      Total += C2
        //    
        //    CheckDigit = (10 - (Total % 10)) % 10
        //    Last digit must match CheckDigit.

        int sum = 0;
        for (int i = 0; i < 9; i++)
        {
            int digit = cleaned[i] - '0';
            int c1 = (digit + (10 - (i + 1))) % 10;
            int c2;

            if (c1 == 9)
            {
                c2 = 9;
            }
            else
            {
                c2 = (c1 * (int)Math.Pow(2, 10 - (i + 1))) % 9;
            }

            sum += c2;
        }

        int checkDigit = (10 - (sum % 10)) % 10;
        int lastDigit = cleaned[9] - '0';

        if (lastDigit != checkDigit)
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
