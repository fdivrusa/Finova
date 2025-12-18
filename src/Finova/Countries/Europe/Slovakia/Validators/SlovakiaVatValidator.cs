using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Enterprise;
using Finova.Core.Identifiers;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Slovakia.Validators;

public partial class SlovakiaVatValidator : IVatValidator, ITaxIdValidator
{
    [GeneratedRegex(@"^\d{10}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "SK";

    public string CountryCode => VatPrefix;
    public EnterpriseNumberType Type => EnterpriseNumberType.SlovakiaVat;

    ValidationResult IValidator<VatDetails>.Validate(string? instance) => Validate(instance);

    public ValidationResult Validate(string? number) => ValidateVat(number);

    public VatDetails? Parse(string? vat) => GetVatDetails(vat);

    string? IValidator<string>.Parse(string? instance) => Normalize(instance);

    public static ValidationResult ValidateVat(string? vat)
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
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidSlovakiaVatFormat);
        }

        if (!long.TryParse(cleaned, out long numericValue))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidSlovakiaVatFormatNonNumeric);
        }

        if (numericValue % 11 != 0)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidSlovakiaVatChecksum);
        }

        return ValidationResult.Success();
    }

    public static VatDetails? GetVatDetails(string? vat)
    {
        var result = ValidateVat(vat);
        if (!result.IsValid)
        {
            return null;
        }

        var cleaned = VatSanitizer.Sanitize(vat)!.Trim().ToUpperInvariant();
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

    public string? Normalize(string? number)
    {
        if (string.IsNullOrWhiteSpace(number)) return null;
        var cleaned = number.ToUpperInvariant().Replace(VatPrefix, "").Replace(" ", "");
        return VatRegex().IsMatch(cleaned) ? cleaned : null;
    }
}
