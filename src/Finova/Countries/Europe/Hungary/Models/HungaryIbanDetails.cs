using Finova.Core.Iban;

namespace Finova.Countries.Europe.Hungary.Models;

/// <summary>
/// Hungary specific IBAN details.
/// HU IBAN format: HU + 2 check + 3 Bank + 4 Branch + 1 Check + 15 Account + 1 Check.
/// </summary>
public record HungaryIbanDetails : IbanDetails
{
    /// <summary>
    /// Bankazonosító (Bank Code - 3 digits).
    /// </summary>
    public required string Bankazonosito { get; init; }

    /// <summary>
    /// Fiókazonosító (Branch Code - 4 digits).
    /// </summary>
    public required string Fiokazonosito { get; init; }

    /// <summary>
    /// Számlaszám (Account Number - 17 digits total including internal checks).
    /// Includes the 15-digit account and internal check digits.
    /// </summary>
    public required string Szamlaszam { get; init; }
}
