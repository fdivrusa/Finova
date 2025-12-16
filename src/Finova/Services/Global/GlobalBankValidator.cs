using Finova.Core.Common;
using Finova.Countries.NorthAmerica.UnitedStates.Validators;
using Finova.Countries.NorthAmerica.Canada.Validators;
using Finova.Countries.Asia.Singapore.Validators;
using Finova.Countries.Asia.Japan.Validators;
using Finova.Countries.Europe.UnitedKingdom.Validators;
using Finova.Countries.Oceania.Australia.Validators;

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
            "US" => UnitedStatesRoutingNumberValidator.ValidateRoutingNumber(routingNumber),
            "CA" => new CanadaRoutingNumberValidator().Validate(routingNumber),
            "GB" => new UnitedKingdomSortCodeValidator().Validate(routingNumber),
            "UK" => new UnitedKingdomSortCodeValidator().Validate(routingNumber), // Alias
            "AU" => new AustraliaBsbValidator().Validate(routingNumber),
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
            "SG" => new SingaporeBankAccountValidator().Validate(accountNumber),
            "JP" => new JapanBankAccountValidator().Validate(accountNumber),
            "GB" => new UnitedKingdomBankAccountValidator().Validate(accountNumber),
            "UK" => new UnitedKingdomBankAccountValidator().Validate(accountNumber), // Alias
            "AU" => new AustraliaBankAccountValidator().Validate(accountNumber),
            _ => ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, ValidationMessages.UnsupportedCountry)
        };
    }
}
