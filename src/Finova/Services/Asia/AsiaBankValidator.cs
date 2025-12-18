using Finova.Core.Common;
using Finova.Countries.Asia.Singapore.Validators;
using Finova.Countries.Asia.Japan.Validators;
using Finova.Countries.Asia.China.Validators;
using Finova.Countries.Asia.India.Validators;

namespace Finova.Services;

/// <summary>
/// Unified validator for Asian Bank Routing and Account Numbers.
/// </summary>
public class AsiaBankValidator
{
    /// <summary>
    /// Validates a Bank Routing Number for the specified Asian country.
    /// </summary>
    public static ValidationResult ValidateRoutingNumber(string countryCode, string? routingNumber)
    {
        if (string.IsNullOrWhiteSpace(countryCode))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        return countryCode.ToUpperInvariant() switch
        {
            "CN" => new ChinaCnapsValidator().Validate(routingNumber),
            "IN" => new IndiaIfscValidator().Validate(routingNumber),
            _ => ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, ValidationMessages.UnsupportedCountry)
        };
    }

    /// <summary>
    /// Validates a Bank Account Number for the specified Asian country.
    /// </summary>
    public static ValidationResult ValidateBankAccount(string countryCode, string? accountNumber)
    {
        if (string.IsNullOrWhiteSpace(countryCode))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        return countryCode.ToUpperInvariant() switch
        {
            "SG" => new SingaporeBankAccountValidator().Validate(accountNumber),
            "JP" => new JapanBankAccountValidator().Validate(accountNumber),
            _ => ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, ValidationMessages.UnsupportedCountry)
        };
    }

    public ValidationResult ValidateRouting(string countryCode, string? routingNumber) => ValidateRoutingNumber(countryCode, routingNumber);
    public ValidationResult ValidateAccount(string countryCode, string? accountNumber) => ValidateBankAccount(countryCode, accountNumber);
}
