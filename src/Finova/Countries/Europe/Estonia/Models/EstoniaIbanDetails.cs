using Finova.Core.Iban;

namespace Finova.Countries.Europe.Estonia.Models;

/// <summary>
/// Estonia specific IBAN details.
/// EE IBAN format: EE + 2 check + 2 Bank Code + 14 Account Number.
/// </summary>
public record EstoniaIbanDetails : IbanDetails
{
    /// <summary>
    /// Pangakood (Bank Code - 2 digits).
    /// Identifies the bank.
    /// </summary>
    public required string Pangakood { get; init; }

    /// <summary>
    /// Kontonumber (Account Number - 14 digits).
    /// Contains the account ID and a check digit.
    /// </summary>
    public required string Kontonumber { get; init; }
}
