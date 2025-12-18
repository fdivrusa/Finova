using Finova.Core.Common;

namespace Finova.Services;

/// <summary>
/// Unified validator for Global Bank Routing and Account Numbers.
/// Delegates validation to specific country validators.
/// </summary>
public static class GlobalBankValidator
{
    /// <summary>
    /// Validates a Bank Routing Number for the specified country.
    /// </summary>
    /// <param name="countryCode">The 2-letter ISO country code.</param>
    /// <param name="routingNumber">The routing number to validate.</param>
    public static ValidationResult ValidateRoutingNumber(string countryCode, string? routingNumber)
    {
        if (string.IsNullOrWhiteSpace(countryCode))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        return countryCode.ToUpperInvariant() switch
        {
            // Europe
            "DE" or "FR" or "IT" or "ES" or "GB" or "UK" => EuropeBankValidator.ValidateRoutingNumber(countryCode, routingNumber),

            // North America
            "US" or "CA" => NorthAmericaBankValidator.ValidateRoutingNumber(countryCode, routingNumber),

            // Asia
            "CN" or "IN" => AsiaBankValidator.ValidateRoutingNumber(countryCode, routingNumber),

            // Oceania
            "AU" => OceaniaBankValidator.ValidateRoutingNumber(countryCode, routingNumber),

            // South America
            "BR" => SouthAmericaBankValidator.ValidateRoutingNumber(countryCode, routingNumber),

            _ => ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, ValidationMessages.UnsupportedCountry)
        };
    }

    /// <summary>
    /// Validates a Bank Account Number for the specified country.
    /// </summary>
    /// <param name="countryCode">The 2-letter ISO country code.</param>
    /// <param name="accountNumber">The account number to validate.</param>
    public static ValidationResult ValidateBankAccount(string countryCode, string? accountNumber)
    {
        if (string.IsNullOrWhiteSpace(countryCode))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        return countryCode.ToUpperInvariant() switch
        {
            // Europe
            "GB" or "UK" => EuropeBankValidator.ValidateBankAccount(countryCode, accountNumber),

            // Asia
            "SG" or "JP" => AsiaBankValidator.ValidateBankAccount(countryCode, accountNumber),

            // Oceania
            "AU" => OceaniaBankValidator.ValidateBankAccount(countryCode, accountNumber),

            _ => ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, ValidationMessages.UnsupportedCountry)
        };
    }

    /// <summary>
    /// Validates a BBAN (Basic Bank Account Number) for the specified country.
    /// </summary>
    public static ValidationResult ValidateBban(string countryCode, string? bban)
    {
        if (string.IsNullOrWhiteSpace(countryCode))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        return countryCode.ToUpperInvariant() switch
        {
            // Europe
            "DE" or "FR" or "IT" or "ES" or "GB" or "UK" => EuropeBankValidator.ValidateBban(countryCode, bban),
            _ => ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, ValidationMessages.UnsupportedCountry)
        };
    }
}
