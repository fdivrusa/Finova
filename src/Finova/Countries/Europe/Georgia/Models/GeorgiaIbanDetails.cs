using Finova.Core.Iban;


namespace Finova.Countries.Europe.Georgia.Models;

/// <summary>
/// Georgia-specific IBAN details.
/// GE IBAN format: GE + 2 check digits + 2 bank code + 16 account number.
/// Example: GE6329NB00000001019049
/// </summary>
public record GeorgiaIbanDetails : IbanDetails
{
    /// <summary>
    /// Gets the 2-character bank code (Bankis Kodi).
    /// </summary>
    public required string BankisKodi { get; init; }

    /// <summary>
    /// Gets the 16-digit account number (Angarishis Nomeri).
    /// </summary>
    public required string AngarishisNomeri { get; init; }
}

