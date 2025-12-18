using Finova.Core.Common;
using Finova.Countries.NorthAmerica.UnitedStates.Validators;
using Finova.Countries.NorthAmerica.Canada.Validators;

namespace Finova.Services;

/// <summary>
/// Unified validator for North American Bank Routing and Account Numbers.
/// </summary>
public class NorthAmericaBankValidator
{
    /// <summary>
    /// Validates a Bank Routing Number for the specified North American country.
    /// </summary>
    public static ValidationResult ValidateRoutingNumber(string countryCode, string? routingNumber)
    {
        if (string.IsNullOrWhiteSpace(countryCode))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        return countryCode.ToUpperInvariant() switch
        {
            "US" => UnitedStatesRoutingNumberValidator.ValidateRoutingNumber(routingNumber),
            "CA" => CanadaRoutingNumberValidator.ValidateStatic(routingNumber),
            _ => ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, ValidationMessages.UnsupportedCountry)
        };
    }

    /// <summary>
    /// Validates a Bank Account Number for the specified North American country.
    /// </summary>
    public static ValidationResult ValidateBankAccount(string countryCode, string? accountNumber)
    {
        // Currently no specific account number validation for US/CA in this library
        return ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, ValidationMessages.UnsupportedCountry);
    }

    public ValidationResult ValidateRouting(string countryCode, string? routingNumber) => ValidateRoutingNumber(countryCode, routingNumber);
    public ValidationResult ValidateAccount(string countryCode, string? accountNumber) => ValidateBankAccount(countryCode, accountNumber);
}
