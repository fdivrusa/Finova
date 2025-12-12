using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Austria.Validators;

public partial class AustriaVatValidator : IVatValidator
{
    private const string CountryCodePrefix = "AT";

    [GeneratedRegex(@"^ATU\d{8}$")]
    private static partial Regex AustriaVatRegex();

    public string CountryCode => CountryCodePrefix;

    ValidationResult IValidator<VatDetails>.Validate(string? instance) => Validate(instance);

    public VatDetails? Parse(string? vat) => GetVatDetails(vat);

    public static ValidationResult Validate(string? vat)
    {
        vat = VatSanitizer.Sanitize(vat);
        if (string.IsNullOrWhiteSpace(vat)) return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "Empty.");

        var normalized = vat.Trim().ToUpperInvariant();
        if (normalized.StartsWith("U") && normalized.Length == 9)
        {
            normalized = "AT" + normalized;
        }
        else if (normalized.Length == 8 && long.TryParse(normalized, out _))
        {
            normalized = "ATU" + normalized;
        }
        else if (normalized.StartsWith("AT") && !normalized.StartsWith("ATU"))
        {
            normalized = normalized.Insert(2, "U");
        }


        if (!AustriaVatRegex().IsMatch(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid format. Expected ATU + 8 digits.");
        }

        string digitsStr = normalized.Substring(3, 7);
        int checkDigit = normalized[10] - '0';

        int[] weights = { 1, 2, 1, 2, 1, 2, 1 };

        int sum = ChecksumHelper.CalculateLuhnStyleWeightedSum(digitsStr, weights);

        int total = (sum + 4);
        int remainder = total % 10;
        int calculated = (10 - remainder) % 10;

        return calculated == checkDigit
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid checksum.");
    }

    public static VatDetails? GetVatDetails(string? vat)
    {
        vat = VatSanitizer.Sanitize(vat);

        if (!Validate(vat).IsValid)
        {
            return null;
        }

        var normalized = vat!.Trim().ToUpperInvariant();
        if (!normalized.StartsWith(CountryCodePrefix))
        {
            if (normalized.StartsWith("U"))
            {
                normalized = CountryCodePrefix + normalized;
            }
        }

        return new VatDetails
        {
            VatNumber = normalized,
            CountryCode = CountryCodePrefix,
            IsValid = true
        };
    }
}
