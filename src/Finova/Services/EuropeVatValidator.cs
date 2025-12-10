using Finova.Belgium.Validators;
using Finova.Countries.Europe.France.Validators;
using Finova.Countries.Europe.Italy.Validators;
using Finova.Core.Common;
using Finova.Core.Vat;


namespace Finova.Services;

public class EuropeVatValidator : IVatValidator
{
    public string CountryCode => "";

    public ValidationResult Validate(string? vat) => ValidateVat(vat);

    public VatDetails? Parse(string? vat)
    {
        if (string.IsNullOrWhiteSpace(vat) || vat.Length < 2)
        {
            return null;
        }

        string country = vat[0..2].ToUpperInvariant();

        return country switch
        {
            "BE" => BelgiumVatValidator.GetVatDetails(vat),
            "FR" => FranceVatValidator.GetVatDetails(vat),
            "IT" => ItalyVatValidator.GetVatDetails(vat),
            _ => null
        };
    }

    public static ValidationResult ValidateVat(string? vat)
    {
        if (string.IsNullOrWhiteSpace(vat) || vat.Length < 2)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "VAT number cannot be null or empty.");
        }

        string country = vat[0..2].ToUpperInvariant();

        return country switch
        {
            "BE" => BelgiumVatValidator.Validate(vat),
            "FR" => FranceVatValidator.ValidateFranceVat(vat),
            "IT" => ItalyVatValidator.ValidateItalyVat(vat),
            _ => ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, "Unsupported country code.")
        };
    }
}

