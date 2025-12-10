using Finova.Core.Iban;


using Finova.Countries.Europe.Ireland.Models;
using Finova.Countries.Europe.Ireland.Validators;

namespace Finova.Countries.Europe.Ireland.Services;

public class IrelandIbanParser(IrelandIbanValidator validator) : IIbanParser
{
    private readonly IrelandIbanValidator _validator = validator;
    public string? CountryCode => _validator.CountryCode;

    /// <summary>
    /// Creates a new parser instance with a default validator.
    /// Use this for non-DI scenarios or quick one-off parsing.
    /// </summary>
    public static IrelandIbanParser Create() => new IrelandIbanParser(new IrelandIbanValidator());

    public IbanDetails? ParseIban(string? iban)
    {
        if (!_validator.Validate(iban).IsValid)
        {
            return null;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        // Parsing logic based on Irish offsets:
        // 0-2: Country (IE)
        // 2-4: Check Digits
        // 4-8: Bank Code / BIC (4 chars)
        // 8-14: Sort Code (6 digits)
        // 14-22: Account Number (8 digits)

        return new IrelandIbanDetails
        {
            Iban = normalized,
            CountryCode = normalized[0..2],      // "IE"
            CheckDigits = normalized[2..4],      // Check digits

            // Specific properties
            BankIdentifier = normalized[4..8],   // 4 chars (BIC)
            SortCode = normalized[8..14],        // 6 digits (Branch)
            DomesticAccountNumber = normalized[14..22], // 8 digits (Account)

            // Generic properties (Mapping)
            BankCode = normalized[4..8],         // Maps to BankIdentifier
            BranchCode = normalized[8..14],      // Maps to SortCode
            AccountNumber = normalized[14..22],  // Maps to DomesticAccountNumber

            IsValid = true
        };
    }
}
