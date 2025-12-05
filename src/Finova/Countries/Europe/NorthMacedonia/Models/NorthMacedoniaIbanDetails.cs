using Finova.Core.Models;

namespace Finova.Countries.Europe.NorthMacedonia.Models;

/// <summary>
/// Represents the details of a North Macedonia IBAN.
/// </summary>
/// <summary>
/// Represents the details of a North Macedonia IBAN.
/// </summary>
/// <param name="SifraBanka">The 3-digit bank code.</param>
/// <param name="BrojSmetka">The 10-character account number.</param>
/// <param name="KontrolenBroj">The 2-digit national check digits.</param>
public record NorthMacedoniaIbanDetails : IbanDetails
{
    /// <summary>
    /// Gets the 3-digit bank code.
    /// </summary>
    public required string SifraBanka { get; init; }

    /// <summary>
    /// Gets the 10-character account number.
    /// </summary>
    public required string BrojSmetka { get; init; }

    /// <summary>
    /// Gets the 2-digit national check digits.
    /// </summary>
    public required string KontrolenBroj { get; init; }
}
