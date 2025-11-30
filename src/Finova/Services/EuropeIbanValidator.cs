using Finova.Core.Accounts;
using Finova.Core.Interfaces;
using Finova.Countries.Europe.Belgium.Validators;
using Finova.Countries.Europe.France.Validators;
using Finova.Countries.Europe.Germany.Validators;
using Finova.Countries.Europe.Italy.Validators;
using Finova.Countries.Europe.Luxembourg.Validators;
using Finova.Countries.Europe.Netherlands.Validators;
using Finova.Countries.Europe.Spain.Validators;
using Finova.Countries.Europe.UnitedKingdom.Validators;

namespace Finova.Services;

public class EuropeIbanValidator : IIbanValidator
{
    public string CountryCode => "EU";
    public static bool Validate(string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban) || iban.Length < 2)
        {
            return false;
        }

        string country = iban[0..2].ToUpperInvariant();

        return country switch
        {
            // Route to specific static methods
            "DE" => GermanyIbanValidator.ValidateGermanyIban(iban),
            "IT" => ItalyIbanValidator.ValidateItalyIban(iban),
            "ES" => SpainIbanValidator.ValidateSpainIban(iban),
            "FR" => FranceIbanValidator.ValidateFranceIban(iban),
            "BE" => BelgiumIbanValidator.ValidateBelgiumIban(iban),
            "NL" => NetherlandsIbanValidator.ValidateNetherlandsIban(iban),
            "GB" => UnitedKingdomIbanValidator.ValidateUnitedKingdomIban(iban),
            "LU" => LuxembourgIbanValidator.ValidateLuxembourgIban(iban),

            // Fallback for generic SEPA
            "AT" or "PT" or "IE" or "GR" or "FI" or
            "SE" or "DK" or "NO" or "PL" or "CZ" or "HU" or "RO" or
            "BG" or "HR" or "SI" or "SK" or "EE" or "LV" or "LT" or
            "CY" or "MT" or "GB" or "CH" =>
                IbanHelper.IsValidIban(iban),

            _ => false
        };
    }

    public bool IsValidIban(string? iban)
    {
        return Validate(iban);
    }
}
