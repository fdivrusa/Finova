using Finova.Core.Models;

namespace Finova.Countries.Europe.Denmark.Models;

/// <summary>
/// Denmark-specific IBAN details.
/// DK IBAN format: DK + 2 check + 4 Reg. No + 10 Account.
/// </summary>
public record DenmarkIbanDetails : IbanDetails
{
    /// <summary>
    /// Registreringsnummer (Reg. No - 4 digits).
    /// Identifies the bank branch.
    /// </summary>
    public required string Registreringsnummer { get; init; }

    /// <summary>
    /// Kontonummer (Account Number - 10 digits).
    /// </summary>
    public required string Kontonummer { get; init; }
}
