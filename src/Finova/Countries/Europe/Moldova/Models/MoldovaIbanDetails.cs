using Finova.Core.Iban;

namespace Finova.Countries.Europe.Moldova.Models;

/// <summary>
/// Represents the details of a Moldovan IBAN.
/// </summary>
/// <summary>
/// Represents the details of a Moldovan IBAN.
/// </summary>
/// <param name="CodBanca">The 2-character bank code.</param>
/// <param name="NumarCont">The 18-character account number.</param>
public record MoldovaIbanDetails : IbanDetails
{
    /// <summary>
    /// Gets the 2-character bank code.
    /// </summary>
    public required string CodBanca { get; init; }

    /// <summary>
    /// Gets the 18-character account number.
    /// </summary>
    public required string NumarCont { get; init; }
}
