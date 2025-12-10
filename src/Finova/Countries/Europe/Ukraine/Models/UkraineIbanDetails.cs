using Finova.Core.Iban;


namespace Finova.Countries.Europe.Ukraine.Models;

/// <summary>
/// Ukraine-specific IBAN details.
/// UA IBAN format: UA + 2 check digits + 6 bank code (MFO) + 19 account number.
/// Example: UA102139962200000260072335660
/// </summary>
public record UkraineIbanDetails : IbanDetails
{
    /// <summary>
    /// Gets the 6-digit bank code (Kod Banku / MFO).
    /// </summary>
    public required string KodBanku { get; init; }

    /// <summary>
    /// Gets the 19-character account number (Nomera Rahunku).
    /// </summary>
    public required string NomeraRahunku { get; init; }
}

