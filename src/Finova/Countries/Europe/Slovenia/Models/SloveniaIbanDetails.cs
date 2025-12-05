using Finova.Core.Models;

namespace Finova.Countries.Europe.Slovenia.Models;

/// <summary>
/// Slovenia specific IBAN details.
/// SI IBAN format: SI + 2 check + 5 Bank + 8 Account + 2 Internal Check.
/// </summary>
public record SloveniaIbanDetails : IbanDetails
{
    /// <summary>
    /// Številka banke (Bank Number - 5 digits).
    /// </summary>
    public required string StevilkaBanke { get; init; }

    /// <summary>
    /// Številka računa (Account Number - 8 digits).
    /// </summary>
    public required string StevilkaRacuna { get; init; }

    /// <summary>
    /// Kontrolna številka (Control Number - 2 digits).
    /// Internal BBAN check digits.
    /// </summary>
    public required string KontrolnaStevilka { get; init; }
}
