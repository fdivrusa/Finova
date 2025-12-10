using Finova.Core.Iban;


namespace Finova.Countries.Europe.Kosovo.Models;

/// <summary>
/// Kosovo-specific IBAN details.
/// XK IBAN format: XK + 2 check digits + 2 bank code + 2 branch code + 10 account number + 2 check digits.
/// Example: XK051212012345678906
/// </summary>
public record KosovoIbanDetails : IbanDetails
{
    /// <summary>
    /// Gets the 2-digit bank code (Kodi i bankës).
    /// </summary>
    public required string KodiBankes { get; init; }

    /// <summary>
    /// Gets the 2-digit branch code (Kodi i degës).
    /// </summary>
    public required string KodiDeges { get; init; }

    /// <summary>
    /// Gets the 10-digit account number (Numri i llogarisë).
    /// </summary>
    public required string NumriLlogarise { get; init; }

    /// <summary>
    /// Gets the 2-digit control number (Shifra e kontrollit).
    /// </summary>
    public required string ShifraKontrollit { get; init; }
}

