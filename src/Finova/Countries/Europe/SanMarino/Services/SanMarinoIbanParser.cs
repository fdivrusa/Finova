using Finova.Core.Accounts;
using Finova.Core.Interfaces;
using Finova.Core.Models;
using Finova.Countries.Europe.SanMarino.Models;
using Finova.Countries.Europe.SanMarino.Validators;

namespace Finova.Countries.Europe.SanMarino.Services;

public class SanMarinoIbanParser(SanMarinoIbanValidator validator) : IIbanParser
{
    private readonly SanMarinoIbanValidator _validator = validator;
    public string? CountryCode => _validator.CountryCode;

    /// <summary>
    /// Creates a new parser instance with a default validator.
    /// Use this for non-DI scenarios or quick one-off parsing.
    /// </summary>
    public static SanMarinoIbanParser Create()
    {
        return new SanMarinoIbanParser(new SanMarinoIbanValidator());
    }

    public IbanDetails? ParseIban(string? iban)
    {
        if (!_validator.IsValidIban(iban))
        {
            return null;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        // Parsing logic based on San Marino offsets:
        // 0-2: Country (SM)
        // 2-4: Check Digits
        // 4-5: CIN (1 char)
        // 5-10: ABI (5 chars)
        // 10-15: CAB (5 chars)
        // 15-27: Account (12 chars)

        return new SanMarinoIbanDetails
        {
            Iban = normalized,
            CountryCode = normalized[0..2],      // "SM"
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
