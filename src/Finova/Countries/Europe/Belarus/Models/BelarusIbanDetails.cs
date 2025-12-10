using Finova.Core.Iban;


namespace Finova.Countries.Europe.Belarus.Models;

/// <summary>
/// Belarus-specific IBAN details.
/// BY IBAN format: BY + 2 check digits + 4 bank code (BIC) + 4 balance account + 16 account number.
/// Example: BY7786AKBB1010000000296600
/// </summary>
public record BelarusIbanDetails : IbanDetails
{
    /// <summary>
    /// Gets the 4-character bank code (Kod Banku / BIC).
    /// </summary>
    public required string KodBanku { get; init; }

    /// <summary>
    /// Gets the 4-digit balance account number (Balansovy Rahunak).
    /// </summary>
    public required string BalansovyRahunak { get; init; }

    /// <summary>
    /// Gets the 16-character account number (Numar Rahunku).
    /// </summary>
    public required string NumarRahunku { get; init; }
}

