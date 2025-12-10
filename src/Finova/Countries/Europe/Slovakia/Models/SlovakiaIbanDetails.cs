using Finova.Core.Iban;

namespace Finova.Countries.Europe.Slovakia.Models;

/// <summary>
/// Slovakia specific IBAN details.
/// SK IBAN format: SK + 2 check + 4 Bank Code + 6 Prefix + 10 Account Number.
/// </summary>
public record SlovakiaIbanDetails : IbanDetails
{
    /// <summary>
    /// Kód banky (Bank Code - 4 digits).
    /// </summary>
    public required string KodBanky { get; init; }

    /// <summary>
    /// Predčíslie (Account Prefix - 6 digits).
    /// </summary>
    public required string Predcislie { get; init; }

    /// <summary>
    /// Číslo účtu (Account Number - 10 digits).
    /// </summary>
    public required string CisloUctu { get; init; }
}
