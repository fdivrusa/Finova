using Finova.Core.Iban;


using Finova.Countries.Europe.Italy.Models;
using Finova.Countries.Europe.Italy.Validators;

namespace Finova.Countries.Europe.Italy.Services;

public class ItalyIbanParser(ItalyIbanValidator validator) : IIbanParser
{
    private readonly ItalyIbanValidator _validator = validator;
    public string? CountryCode => _validator.CountryCode;

    /// <summary>
    /// Creates a new parser instance with a default validator.
    /// Use this for non-DI scenarios or quick one-off parsing.
    /// </summary>
    public static ItalyIbanParser Create() => new ItalyIbanParser(new ItalyIbanValidator());

    public IbanDetails? ParseIban(string? iban)
    {
        if (!_validator.Validate(iban).IsValid)
        {
            return null;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        // Parsing logic based on Italian offsets:
        // 0-2: Country (IT)
        // 2-4: Check Digits
        // 4-5: CIN (1 char)
        // 5-10: ABI (5 chars)
        // 10-15: CAB (5 chars)
        // 15-27: Account (12 chars)

        return new ItalyIbanDetails
        {
            Iban = normalized,
            CountryCode = normalized[0..2],      // "IT"
            CheckDigits = normalized[2..4],      // Check digits

            // Specific properties
            Cin = normalized[4..5],              // 1 char
            Abi = normalized[5..10],             // 5 digits (Bank)
            Cab = normalized[10..15],            // 5 digits (Branch)
            NumeroConto = normalized[15..27],    // 12 alphanumeric

            // Generic properties (Mapping)
            BankCode = normalized[5..10],        // ABI maps to BankCode
            BranchCode = normalized[10..15],     // CAB maps to BranchCode
            AccountNumber = normalized[15..27],  // NumeroConto maps to AccountNumber

            IsValid = true
        };
    }
}
