using Finova.Core.Models;

namespace Finova.Countries.Europe.Finland.Models;

/// <summary>
/// Finland-specific IBAN details.
/// FI IBAN format: FI + 2 check + 6 Bank + 8 Account.
/// </summary>
public record FinlandIbanDetails : IbanDetails
{
    /// <summary>
    /// Rahalaitostunnus (6 digits).
    /// Bank Identifier
    /// Position: 5-10
    /// </summary>
    public required string Rahalaitostunnus { get; init; }

    /// <summary>
    /// Tilinumero (8 digits).
    /// Account number
    /// Position: 11-18
    /// </summary>
    public required string Tilinumero { get; init; }
}
