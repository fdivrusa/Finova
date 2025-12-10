using Finova.Core.Iban;

namespace Finova.Countries.Europe.Lithuania.Models;

/// <summary>
/// Lithuania specific IBAN details.
/// LT IBAN format: LT + 2 check + 5 Bank Code + 11 Account Number.
/// </summary>
public record LithuaniaIbanDetails : IbanDetails
{
    /// <summary>
    /// Banko Kodas (Bank Code - 5 digits).
    /// </summary>
    public required string BankoKodas { get; init; }

    /// <summary>
    /// Sąskaitos Numeris (Account Number - 11 digits).
    /// </summary>
    public required string SaskaitosNumeris { get; init; }
}
