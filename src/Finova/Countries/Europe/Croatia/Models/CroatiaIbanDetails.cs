using Finova.Core.Models;

namespace Finova.Countries.Europe.Croatia.Models;

/// <summary>
/// Croatia specific IBAN details.
/// HR IBAN format: HR + 2 check + 7 Bank + 10 Account.
/// </summary>
public record CroatiaIbanDetails : IbanDetails
{
    /// <summary>
    /// Vodeći broj banke (Bank Leading Number - 7 digits).
    /// Identifies the bank.
    /// </summary>
    public required string VodeciBrojBanke { get; init; }

    /// <summary>
    /// Broj računa (Account Number - 10 digits).
    /// </summary>
    public required string BrojRacuna { get; init; }
}
