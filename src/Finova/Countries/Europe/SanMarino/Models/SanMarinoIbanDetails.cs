using Finova.Core.Iban;

namespace Finova.Countries.Europe.SanMarino.Models;

/// <summary>
/// San Marino specific IBAN details.
/// SM IBAN format: SM + 2 check + 1 CIN + 5 ABI + 5 CAB + 12 Account.
/// Identical structure to Italy.
/// </summary>
public record SanMarinoIbanDetails : IbanDetails
{
    public required string Cin { get; init; }
    public required string Abi { get; init; }
    public required string Cab { get; init; }
    public required string NumeroConto { get; init; }
}
