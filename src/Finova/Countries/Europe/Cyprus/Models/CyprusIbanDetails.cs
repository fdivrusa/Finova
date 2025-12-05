using Finova.Core.Models;

namespace Finova.Countries.Europe.Cyprus.Models;

/// <summary>
/// Cyprus specific IBAN details.
/// CY IBAN format: CY + 2 check + 3 Bank + 5 Branch + 16 Account.
/// </summary>
public record CyprusIbanDetails : IbanDetails
{
    /// <summary>
    /// Bank Code (3 digits).
    /// </summary>
    public required string BankCodeCy { get; init; }

    /// <summary>
    /// Branch Code (5 digits).
    /// </summary>
    public required string BranchCodeCy { get; init; }

    /// <summary>
    /// Account Number (16 alphanumeric).
    /// </summary>
    public required string AccountNumberCy { get; init; }
}
