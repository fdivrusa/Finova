using Finova.Core.Models;

namespace Finova.Countries.Europe.Sweden.Models;

/// <summary>
/// Sweden-specific IBAN details.
/// SE IBAN format: SE + 2 check + 3 Bank + 17 Account.
/// </summary>
public record SwedenIbanDetails : IbanDetails
{
    /// <summary>
    /// Bankkod (Bank Code - 3 digits).
    /// Identifies the bank.
    /// </summary>
    public required string Bankkod { get; init; }

    /// <summary>
    /// Kontonummer (Account Number - 17 digits).
    /// </summary>
    public required string Kontonummer { get; init; }
}
