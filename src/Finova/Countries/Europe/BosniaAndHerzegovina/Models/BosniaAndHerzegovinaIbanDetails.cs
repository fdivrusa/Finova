using Finova.Core.Iban;


namespace Finova.Countries.Europe.BosniaAndHerzegovina.Models;

/// <summary>
/// Bosnia and Herzegovina-specific IBAN details.
/// BA IBAN format: BA + 2 check digits + 3 bank code + 3 branch code + 8 account number + 2 check digits.
/// Example: BA393933858048002112
/// </summary>
public record BosniaAndHerzegovinaIbanDetails : IbanDetails
{
    /// <summary>
    /// Gets the 3-digit bank code (Broj banke).
    /// </summary>
    public required string BrojBanke { get; init; }

    /// <summary>
    /// Gets the 3-digit branch code (Broj filijale).
    /// </summary>
    public required string BrojFilijale { get; init; }

    /// <summary>
    /// Gets the 8-digit account number (Broj računa).
    /// </summary>
    public required string BrojRacuna { get; init; }

    /// <summary>
    /// Gets the 2-digit control number (Kontrolni broj).
    /// </summary>
    public required string KontrolniBroj { get; init; }
}

