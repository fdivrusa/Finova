using Finova.Core.Iban;

namespace Finova.Countries.Europe.Netherlands.Models;

/// <summary>
/// Dutch-specific IBAN details.
/// NL IBAN format: NL + 2 check digits + 4 bank code + 10 account number.
/// Example: NL91ABNA0417164300
/// </summary>
public record NetherlandsIbanDetails : IbanDetails
{
    /// <summary>
    /// 4-letter bank code.
    /// Position: 5-8
    /// Example: "ABNA" (ABN AMRO), "INGB" (ING), "RABO" (Rabobank)
    /// </summary>
    public required string BankCodeNl { get; init; }

    /// <summary>
    /// 10-digit account number.
    /// Position: 9-18
    /// </summary>
    public required string AccountNumberNl { get; init; }
}
