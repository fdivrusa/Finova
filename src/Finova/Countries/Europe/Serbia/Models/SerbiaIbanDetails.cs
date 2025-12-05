using Finova.Core.Models;

namespace Finova.Countries.Europe.Serbia.Models;

/// <summary>
/// Serbia-specific IBAN details.
/// RS IBAN format: RS + 2 check + 3 Bank + 13 Account + 2 BBAN Check.
/// </summary>
public record SerbiaIbanDetails : IbanDetails
{
    /// <summary>
    /// Broj Banke (Bank Number - 3 digits).
    /// </summary>
    public required string BrojBanke { get; init; }

    /// <summary>
    /// Broj Racuna (Account Number - 13 digits).
    /// </summary>
    public required string BrojRacuna { get; init; }

    /// <summary>
    /// Kontrolni Broj (Control Number - 2 digits).
    /// Internal BBAN check digits.
    /// </summary>
    public required string KontrolniBroj { get; init; }
}
