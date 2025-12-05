using Finova.Core.Models;

namespace Finova.Countries.Europe.Montenegro.Models;

/// <summary>
/// Represents the details of a Montenegro IBAN.
/// </summary>
/// <summary>
/// Represents the details of a Montenegro IBAN.
/// </summary>
/// <param name="SifraBanke">The 3-digit bank code.</param>
/// <param name="BrojRacuna">The 13-digit account number.</param>
/// <param name="KontrolniBroj">The 2-digit national check digits.</param>
public record MontenegroIbanDetails : IbanDetails
{
    /// <summary>
    /// Gets the 3-digit bank code.
    /// </summary>
    public required string SifraBanke { get; init; }

    /// <summary>
    /// Gets the 13-character account number.
    /// </summary>
    public required string BrojRacuna { get; init; }

    /// <summary>
    /// Gets the 2-digit national check digits.
    /// </summary>
    public required string KontrolniBroj { get; init; }
}
