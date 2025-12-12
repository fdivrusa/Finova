using Finova.Core.Common;
using Finova.Core.Vat;
using Finova.Countries.Europe.France.Validators; // Assurez-vous d'avoir ce using

namespace Finova.Countries.Europe.Monaco.Validators;

/// <summary>
/// Validator for Monaco VAT numbers.
/// Monaco is part of the French VAT system.
/// Format: FR + 2 key digits + 9 SIREN digits.
/// Often represented locally with 'MC' prefix, but technical validation is 'FR'.
/// </summary>
public partial class MonacoVatValidator : IVatValidator
{
    private const string CountryCodePrefix = "MC";
    private const string FrenchPrefix = "FR";

    public string CountryCode => CountryCodePrefix;

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
        string frenchFormatVat;

        if (cleaned.StartsWith(CountryCodePrefix))
        {
            frenchFormatVat = string.Concat(FrenchPrefix, cleaned.AsSpan(2));
        }
        else if (cleaned.StartsWith(FrenchPrefix))
        {
            frenchFormatVat = cleaned;
        }
        else
        {
            frenchFormatVat = string.Concat(FrenchPrefix, cleaned);
        }

        return FranceVatValidator.Validate(frenchFormatVat);
    }

    public static VatDetails? GetVatDetails(string? vat)
    {
        var result = Validate(vat);
        if (!result.IsValid)
        {
            return null;
        }

        var cleaned = VatSanitizer.Sanitize(vat)!.Trim().ToUpperInvariant();


        if (cleaned.StartsWith(CountryCodePrefix))
        {
            cleaned = cleaned[2..];
        }
        else if (cleaned.StartsWith(FrenchPrefix))
        {
            cleaned = cleaned[2..];
        }

        return new VatDetails
        {
            CountryCode = CountryCodePrefix,
            VatNumber = cleaned,
            IsValid = true
        };
    }
}