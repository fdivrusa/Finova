using Finova.Core.Iban;


namespace Finova.Countries.Europe.Turkey.Models;

/// <summary>
/// Turkey-specific IBAN details.
/// TR IBAN format: TR + 2 check digits + 5 bank code + 1 reserved + 16 account number.
/// Example: TR943300061005197864578413
/// </summary>
public record TurkeyIbanDetails : IbanDetails
{
    /// <summary>
    /// Gets the 5-digit bank code (Banka Kodu).
    /// </summary>
    public required string BankaKodu { get; init; }

    /// <summary>
    /// Gets the 1-digit reserved field (Rezerv Alan).
    /// </summary>
    public required string RezervAlan { get; init; }

    /// <summary>
    /// Gets the 16-character account number (Hesap Numarası).
    /// </summary>
    public required string HesapNumarasi { get; init; }
}

