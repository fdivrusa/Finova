using Finova.Core.Iban;

namespace Finova.Countries.Europe.CzechRepublic.Models;

/// <summary>
/// Czech Republic specific IBAN details.
/// CZ IBAN format: CZ + 2 check + 4 Bank Code + 6 Prefix + 10 Account Number.
/// </summary>
public record CzechRepublicIbanDetails : IbanDetails
{
    /// <summary>
    /// Kód banky (Bank Code - 4 digits).
    /// Identifies the bank.
    /// </summary>
    public required string KodBanky { get; init; }

    /// <summary>
    /// Předčíslí (Account Prefix - 6 digits).
    /// Prefix of the account number.
    /// </summary>
    public required string Predcisli { get; init; }

    /// <summary>
    /// Číslo účtu (Account Number - 10 digits).
    /// The main account number.
    /// </summary>
    public required string CisloUctu { get; init; }
}
