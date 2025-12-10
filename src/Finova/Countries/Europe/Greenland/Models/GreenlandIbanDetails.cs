using Finova.Core.Iban;


namespace Finova.Countries.Europe.Greenland.Models;

/// <summary>
/// Greenland-specific IBAN details.
/// GL IBAN format: GL + 2 check digits + 4 bank code + 10 account number.
/// Example: GL1589647101234567
/// </summary>
public record GreenlandIbanDetails : IbanDetails
{
    /// <summary>
    /// Gets the 4-digit bank code (Bank kode).
    /// </summary>
    public required string BankKode { get; init; }

    /// <summary>
    /// Gets the 10-digit account number (Kontonummer).
    /// </summary>
    public required string KontoNummer { get; init; }
}

