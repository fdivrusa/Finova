using Finova.Core.Models;

namespace Finova.Countries.Europe.Latvia.Models;

/// <summary>
/// Latvia specific IBAN details.
/// LV IBAN format: LV + 2 check + 4 Bank Code + 13 Account Number.
/// </summary>
public record LatviaIbanDetails : IbanDetails
{
    /// <summary>
    /// Bankas Kods (Bank Code - 4 alphanumeric characters).
    /// Often corresponds to the BIC/SWIFT code.
    /// </summary>
    public required string BankasKods { get; init; }

    /// <summary>
    /// Klienta Konta Numurs (Customer Account Number - 13 alphanumeric characters).
    /// </summary>
    public required string KlientaKontaNumurs { get; init; }
}
