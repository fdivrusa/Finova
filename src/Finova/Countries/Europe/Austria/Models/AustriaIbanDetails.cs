using Finova.Core.Models;

namespace Finova.Countries.Europe.Austria.Models;

/// <summary>
/// Austria-specific IBAN details.
/// AT IBAN format: AT + 2 check + 5 Bankleitzahl + 11 Kontonummer.
/// </summary>
public record AustriaIbanDetails : IbanDetails
{
    /// <summary>
    /// Bankleitzahl (BLZ) - 5 digits.
    /// Austria Bank code
    /// Position: 5-9
    /// </summary>
    public required string Bankleitzahl { get; init; }

    /// <summary>
    /// Kontonummer - 11 digits.
    /// Account number.
    /// Position: 10-20
    /// </summary>
    public required string Kontonummer { get; init; }
}
