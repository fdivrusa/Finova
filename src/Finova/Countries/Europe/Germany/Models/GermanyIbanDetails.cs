using Finova.Core.Iban;

namespace Finova.Countries.Europe.Germany.Models;

/// <summary>
/// German-specific IBAN details.
/// DE IBAN format: DE + 2 check digits + 8 bank code (Bankleitzahl) + 10 account number.
/// Example: DE89370400440532013000
/// </summary>
public record GermanyIbanDetails : IbanDetails
{
    /// <summary>
    /// 8-digit bank code (Bankleitzahl - BLZ).
    /// Position: 5-12
    /// </summary>
    public required string Bankleitzahl { get; init; }

    /// <summary>
    /// 10-digit account number (Kontonummer).
    /// Position: 13-22
    /// </summary>
    public required string Kontonummer { get; init; }
}
