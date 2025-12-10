using Finova.Core.Iban;

namespace Finova.Countries.Europe.Luxembourg.Models;

/// <summary>
/// Luxembourg-specific IBAN details.
/// LU IBAN format: LU + 2 check digits + 3 bank code + 13 account number.
/// Example: LU280019400644750000
/// </summary>
public record LuxembourgIbanDetails : IbanDetails
{
    /// <summary>
    /// 3-digit bank code.
    /// Position: 5-7
    /// </summary>
    public required string BankCodeLu { get; init; }

    /// <summary>
    /// 13-character account number.
    /// Position: 8-20
    /// </summary>
    public required string AccountNumberLu { get; init; }
}
