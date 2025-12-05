using Finova.Core.Models;

namespace Finova.Countries.Europe.Malta.Models;

/// <summary>
/// Malta specific IBAN details.
/// MT IBAN format: MT + 2 check + 4 Bank (BIC) + 5 Sort Code + 18 Account.
/// </summary>
public record MaltaIbanDetails : IbanDetails
{
    /// <summary>
    /// Bank BIC Code (4 letters).
    /// </summary>
    public required string BankBic { get; init; }

    /// <summary>
    /// Sort Code (5 digits).
    /// </summary>
    public required string SortCode { get; init; }

    /// <summary>
    /// Account Number (18 alphanumeric).
    /// </summary>
    public required string AccountNumberMt { get; init; }
}
