using Finova.Core.Common;
using Finova.Core.Enterprise;
using Finova.Countries.Europe.Austria.Validators;
using Finova.Countries.Europe.Belgium.Validators;
using Finova.Countries.Europe.France.Validators;

namespace Finova.Services;

/// <summary>
/// Unified validator for European Enterprise/Business Registration Numbers.
/// Delegates validation to specific country validators based on the country code or enterprise number type.
/// </summary>
public class EuropeEnterpriseValidator : IEnterpriseValidator
{
    public string CountryCode => "";

    public ValidationResult Validate(string? number) => ValidateEnterpriseNumber(number);

    public string? Parse(string? number) => GetNormalizedNumber(number);

    public static ValidationResult ValidateEnterpriseNumber(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "Enterprise number cannot be empty.");
        }

        if (number.Length < 2)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "Number is too short to determine country.");
        }

        string country = number[0..2].ToUpperInvariant();
        return ValidateEnterpriseNumber(number, country);
    }

    public static ValidationResult ValidateEnterpriseNumber(string? number, EnterpriseNumberType type)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "Enterprise number cannot be empty.");
        }

        return type switch
        {
            EnterpriseNumberType.AustriaFirmenbuch => AustriaFirmenbuchValidator.ValidateFirmenbuch(number),
            EnterpriseNumberType.BelgiumEnterpriseNumber => BelgiumEnterpriseValidator.Validate(number),
            EnterpriseNumberType.FranceSiren => FranceSirenValidator.ValidateSiren(number),
            EnterpriseNumberType.FranceSiret => FranceSiretValidator.ValidateSiret(number),
            _ => ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, $"Enterprise number type {type} is not supported.")
        };
    }

    public static ValidationResult ValidateEnterpriseNumber(string? number, string countryCode)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "Enterprise number cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(countryCode))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "Country code cannot be empty.");
        }

        string country = countryCode.ToUpperInvariant();

        switch (country)
        {
            case "AT":
                if (number.StartsWith("AT", StringComparison.OrdinalIgnoreCase))
                {
                    return ValidateEnterpriseNumber(number[2..], EnterpriseNumberType.AustriaFirmenbuch);
                }
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.AustriaFirmenbuch);
            case "BE":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.BelgiumEnterpriseNumber);
            case "FR":
                string frNumber = number;
                if (number.StartsWith("FR", StringComparison.OrdinalIgnoreCase))
                {
                    frNumber = number[2..];
                }

                if (frNumber.Length == 9)
                {
                    return ValidateEnterpriseNumber(frNumber, EnterpriseNumberType.FranceSiren);
                }
                return ValidateEnterpriseNumber(frNumber, EnterpriseNumberType.FranceSiret);
            default:
                return ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, $"Country code {country} is not supported.");
        }
    }

    public static string? GetNormalizedNumber(string? number)
    {
        if (string.IsNullOrWhiteSpace(number) || number.Length < 2)
        {
            return null;
        }

        string country = number[0..2].ToUpperInvariant();

        return country switch
        {
            "AT" => new AustriaFirmenbuchValidator().Parse(number[2..]),
            "BE" => new BelgiumEnterpriseValidator().Parse(number),
            "FR" => number.Length == 11 ? new FranceSirenValidator().Parse(number[2..]) : new FranceSiretValidator().Parse(number[2..]),
            _ => null
        };
    }
}
