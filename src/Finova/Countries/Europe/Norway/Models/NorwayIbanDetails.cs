using Finova.Core.Iban;

namespace Finova.Countries.Europe.Norway.Models;

/// <summary>
/// Norway-specific IBAN details.
/// NO IBAN format: NO + 2 check + 4 Bank + 6 Account + 1 Control Digit.
/// </summary>
public record NorwayIbanDetails : IbanDetails
{
    /// <summary>
    /// Bankkod (Bank Code - 4 digits).
    /// </summary>
    public required string Bankkod { get; init; }

    /// <summary>
    /// Kontonummer (Account Number - 6 digits).
    /// </summary>
    public required string Kontonummer { get; init; }

    /// <summary>
    /// Kontrollsiffer (Control Digit - 1 digit).
    /// Internal Modulo 11 check digit.
    /// </summary>
    public required string Kontrollsiffer { get; init; }
}
