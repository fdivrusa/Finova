using Finova.Core.Iban;


namespace Finova.Countries.Europe.Azerbaijan.Models;

/// <summary>
/// Azerbaijan-specific IBAN details.
/// AZ IBAN format: AZ + 2 check digits + 4 bank code + 20 account number.
/// Example: AZ9221NABZ000000001370100019
/// </summary>
public record AzerbaijanIbanDetails : IbanDetails
{
    /// <summary>
    /// Gets the 4-character bank code (Bank Kodu).
    /// </summary>
    public required string BankKodu { get; init; }

    /// <summary>
    /// Gets the 20-digit account number (Hesab Nömrəsi).
    /// </summary>
    public required string HesabNomresi { get; init; }
}

