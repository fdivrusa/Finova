using Finova.Core.Models;

namespace Finova.Countries.Europe.Gibraltar.Models;

/// <summary>
/// Gibraltar specific IBAN details.
/// GI IBAN format: GI + 2 check + 4 Bank (BIC) + 15 Account.
/// </summary>
public record GibraltarIbanDetails : IbanDetails
{
    /// <summary>
    /// Bank Code / BIC (4 letters).
    /// </summary>
    public required string BankCodeGi { get; init; }

    /// <summary>
    /// Account Number (15 alphanumeric).
    /// </summary>
    public required string AccountNumberGi { get; init; }
}
