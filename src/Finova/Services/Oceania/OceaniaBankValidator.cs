using Finova.Core.Common;
using Finova.Countries.Oceania.Australia.Validators;

namespace Finova.Services;

/// <summary>
/// Unified validator for Oceania Bank Routing and Account Numbers.
/// </summary>
public class OceaniaBankValidator
{
    /// <summary>
    /// Validates a Bank Routing Number for the specified Oceania country.
    /// </summary>
    public static ValidationResult ValidateRoutingNumber(string countryCode, string? routingNumber)
    {
        if (string.IsNullOrWhiteSpace(countryCode))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        return countryCode.ToUpperInvariant() switch
        {
            "AU" => AustraliaBsbValidator.ValidateStatic(routingNumber),
            _ => ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, ValidationMessages.UnsupportedCountry)
        };
    }

    /// <summary>
    /// Validates a Bank Account Number for the specified Oceania country.
    /// </summary>
    public static ValidationResult ValidateBankAccount(string countryCode, string? accountNumber)
    {
        if (string.IsNullOrWhiteSpace(countryCode))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        return countryCode.ToUpperInvariant() switch
        {
            "AU" => new AustraliaBankAccountValidator().Validate(accountNumber),
            _ => ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, ValidationMessages.UnsupportedCountry)
        };
    }

    public ValidationResult ValidateRouting(string countryCode, string? routingNumber) => ValidateRoutingNumber(countryCode, routingNumber);
    public ValidationResult ValidateAccount(string countryCode, string? accountNumber) => ValidateBankAccount(countryCode, accountNumber);
}
