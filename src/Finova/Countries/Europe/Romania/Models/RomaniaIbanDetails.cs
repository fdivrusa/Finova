using Finova.Core.Models;

namespace Finova.Countries.Europe.Romania.Models;

/// <summary>
/// Romania specific IBAN details.
/// RO IBAN format: RO + 2 check + 4 Bank + 16 Account.
/// </summary>
public record RomaniaIbanDetails : IbanDetails
{
    /// <summary>
    /// Codul Bancii / BIC (Bank Code - 4 alphanumeric characters).
    /// Usually the first 4 characters of the BIC.
    /// </summary>
    public required string CodulBancii { get; init; }

    /// <summary>
    /// Numar Cont (Account Number - 16 alphanumeric characters).
    /// </summary>
    public required string NumarCont { get; init; }
}
