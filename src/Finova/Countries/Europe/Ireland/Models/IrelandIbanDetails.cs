using Finova.Core.Models;

namespace Finova.Countries.Europe.Ireland.Models;

/// <summary>
/// Irish-specific IBAN details.
/// IE IBAN format: IE + 2 check digits + 4 Bank Code (BIC) + 6 Sort Code + 8 Account Number.
/// Length: 22 characters.
/// Example: IE29AIBK93115212345678
/// </summary>
public record IrelandIbanDetails : IbanDetails
{
    /// <summary>
    /// Bank Identifier Code (BIC) - 4 alphabetic characters.
    /// Represents the bank (e.g., "AIBK" for Allied Irish Banks).
    /// Position: 5-8
    /// </summary>
    public required string BankIdentifier { get; init; }

    /// <summary>
    /// Sort Code (6 digits).
    /// Identifies the specific branch of the bank.
    /// Position: 9-14
    /// </summary>
    public required string SortCode { get; init; }

    /// <summary>
    /// Domestic Account Number (8 digits).
    /// If the original account number is shorter, it is left-padded with zeros.
    /// Position: 15-22
    /// </summary>
    public required string DomesticAccountNumber { get; init; }
}
