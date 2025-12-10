using Finova.Core.Iban;

namespace Finova.Countries.Europe.Switzerland.Models;

/// <summary>
/// Switzerland specific IBAN details.
/// CH IBAN format: CH + 2 check + 5 Clearing Number + 12 Account Number.
/// </summary>
public record SwitzerlandIbanDetails : IbanDetails
{
    /// <summary>
    /// Clearing-Nummer (Clearing Number - 5 digits).
    /// Identifies the financial institution.
    /// </summary>
    public required string ClearingNummer { get; init; }

    /// <summary>
    /// Konto-Nummer (Account Number - 12 alphanumeric).
    /// </summary>
    public required string KontoNummer { get; init; }
}
