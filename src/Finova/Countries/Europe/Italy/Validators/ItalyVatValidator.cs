using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;
using Finova.Countries.Europe.Italy.Models;

namespace Finova.Countries.Europe.Italy.Validators;

public partial class ItalyVatValidator : IVatValidator
{
    [GeneratedRegex(@"[^\d]")]
    private static partial Regex DigitsOnlyRegex();

    private const string CountryCodePrefix = "IT";

    public string CountryCode => CountryCodePrefix;

    ValidationResult IValidator<VatDetails>.Validate(string? instance) => Validate(instance);

    public VatDetails? Parse(string? vat) => GetVatDetails(vat);

    public static ValidationResult Validate(string? vat)
    {
        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = vat.Trim().ToUpperInvariant();

        if (!normalized.StartsWith(CountryCodePrefix))
        {
            if (normalized.Length == 11 && long.TryParse(normalized, out _))
            {
                // Proceed
            }
            else
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, string.Format(ValidationMessages.InvalidVatFormat, "Italy"));
            }
        }
        else
        {
            normalized = normalized[2..];
        }

        normalized = DigitsOnlyRegex().Replace(normalized, "");

        if (normalized.Length != 11)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, 11, normalized.Length));
        }

        if (normalized == "00000000000")
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, string.Format(ValidationMessages.InvalidVatChecksum, "Italy"));
        }

        if (!ChecksumHelper.ValidateLuhn(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, string.Format(ValidationMessages.InvalidVatChecksum, "Italy"));
        }

        return ValidationResult.Success();
    }

    public static ItalyVatDetails? GetVatDetails(string? vat)
    {
        var result = Validate(vat);
        if (!result.IsValid)
        {
            return null;
        }

        var normalized = vat!.Trim().ToUpperInvariant();
        if (normalized.StartsWith(CountryCodePrefix))
        {
            normalized = normalized[2..];
        }
        normalized = DigitsOnlyRegex().Replace(normalized, "");

        var officeCode = normalized.Substring(7, 3);

        return new ItalyVatDetails
        {
            VatNumber = $"{CountryCodePrefix}{normalized}",
            CountryCode = CountryCodePrefix,
            IsValid = true,
            OfficeCode = officeCode
        };
    }
}
