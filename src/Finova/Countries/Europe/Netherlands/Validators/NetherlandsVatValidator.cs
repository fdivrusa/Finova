using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Netherlands.Validators;

public partial class NetherlandsVatValidator : IVatValidator
{
    [GeneratedRegex(@"^\d{9}B\d{2}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "NL";

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
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid Netherlands VAT format.");
        }

        // Checksum Validation
        // 1. Mod 97 (New format / Standard)
        // Convert letters to numbers (A=10, B=11...)
        // NL VAT usually has 'B' at pos 10.

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        foreach (char c in cleaned)
        {
            if (char.IsDigit(c))
            {
                sb.Append(c);
            }
            else
            {
                sb.Append((int)(c - 'A' + 10));
            }
        }
        string mod97Str = sb.ToString();
        bool isMod97Valid = ChecksumHelper.CalculateModulo97(mod97Str) == 1;

        // 2. Elfproef (Old format / RSIN)
        // Weighted Mod 11 on first 9 digits.
        bool isElfproefValid = false;
        if (cleaned.Length >= 9)
        {
            string rsin = cleaned.Substring(0, 9);
            if (long.TryParse(rsin, out _))
            {
                // Weights: 9, 8, 7, 6, 5, 4, 3, 2, -1
                int[] elfWeights = { 9, 8, 7, 6, 5, 4, 3, 2, -1 };
                if (ChecksumHelper.CalculateWeightedModulo11(rsin, elfWeights) == 0)
                {
                    isElfproefValid = true;
                }
            }
        }

        if (!isMod97Valid && !isElfproefValid)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid checksum (Mod 97 or Elfproef).");
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
