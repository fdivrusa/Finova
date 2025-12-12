using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Ireland.Validators;

/// <summary>
/// Validator for Ireland VAT numbers.
/// </summary>
public partial class IrelandVatValidator : IVatValidator
{
    [GeneratedRegex(@"^(\d{7}[A-W][A-I]?|\d[A-Z+*]\d{5}[A-W])$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "IE";
    private static readonly int[] Weights = [8, 7, 6, 5, 4, 3, 2];

    public string CountryCode => VatPrefix;

    ValidationResult IValidator<VatDetails>.Validate(string? instance) => Validate(instance);

    public VatDetails? Parse(string? vat) => GetVatDetails(vat);

    /// <summary>
    /// Validates an Irish VAT number using the weighted Modulo 23 algorithm.
    /// Handles both pre-2013 and post-2013 formats.
    /// </summary>
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
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid Ireland VAT format.");
        }

        int sum;
        char checkChar;

        if (char.IsLetter(cleaned[1]) || cleaned[1] == '+' || cleaned[1] == '*')
        {
            string numberPart = "0" + cleaned.Substring(2, 5) + cleaned[0];
            int extraSum = 9 * GetValue(cleaned[1]);

            sum = ChecksumHelper.CalculateWeightedSum(numberPart, Weights) + extraSum;
            checkChar = cleaned[7];
        }
        else
        {
            string numberPart = cleaned.Substring(0, 7);
            sum = ChecksumHelper.CalculateWeightedSum(numberPart, Weights);

            if (cleaned.Length == 9)
            {
                sum += 9 * GetValue(cleaned[8]);
                checkChar = cleaned[7];
            }
            else
            {
                checkChar = cleaned[7];
            }
        }

        int remainder = sum % 23;
        char expectedChar = remainder == 0 ? 'W' : (char)('A' + remainder - 1);

        if (checkChar != expectedChar)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid Ireland VAT checksum.");
        }

        return ValidationResult.Success();
    }

    private static int GetValue(char c)
    {
        if (char.IsDigit(c))
        {
            return c - '0';
        }

        if (c == '+' || c == '*')
        {
            return 0;
        }

        return c - 'A' + 1;
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