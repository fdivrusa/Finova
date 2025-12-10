using Finova.Core.Iban;

namespace Finova.Countries.Europe.Poland.Models;

/// <summary>
/// Poland-specific IBAN details.
/// PL IBAN format: PL + 2 check + 8 Bank + 16 Account.
/// </summary>
public record PolandIbanDetails : IbanDetails
{
    /// <summary>
    /// Numer Rozliczeniowy Banku (Bank Sorting Code - 8 digits).
    /// </summary>
    public required string NumerRozliczeniowyBanku { get; init; }

    /// <summary>
    /// Numer Rachunku (Account Number - 16 digits).
    /// </summary>
    public required string NumerRachunku { get; init; }
}
