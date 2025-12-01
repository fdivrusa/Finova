using Finova.Core.Models;

namespace Finova.Countries.Europe.Greece.Models;

/// <summary>
/// Greece-specific IBAN details.
/// GR IBAN format: GR + 2 check + 3 Bank (HEBIC) + 4 Branch + 16 Account.
/// </summary>
public record GreeceIbanDetails : IbanDetails
{
    /// <summary>
    /// Kodikos Trapezas (3 digits).
    /// Bank code
    /// Position: 5-7
    /// </summary>
    public required string KodikosTrapezas { get; init; }

    /// <summary>
    /// Kodikos Katastimatos (4 digits).
    /// Agency code
    /// Position: 8-11
    /// </summary>
    public required string KodikosKatastimatos { get; init; }

    /// <summary>
    /// Arithmos Logariasmou (16 alphanumeric characters).
    /// Account number
    /// Position: 12-27
    /// </summary>
    public required string ArithmosLogariasmou { get; init; }
}
