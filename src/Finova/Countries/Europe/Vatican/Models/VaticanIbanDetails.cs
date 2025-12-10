using Finova.Core.Iban;

namespace Finova.Countries.Europe.Vatican.Models;

/// <summary>
/// Vatican City specific IBAN details.
/// VA IBAN format: VA + 2 check + 3 Bank + 15 Account.
/// </summary>
public record VaticanIbanDetails : IbanDetails
{
    /// <summary>
    /// Codice Banca (Bank Code - 3 digits).
    /// </summary>
    public required string CodiceBanca { get; init; }

    /// <summary>
    /// Numero Conto (Account Number - 15 digits).
    /// </summary>
    public required string NumeroConto { get; init; }
}
