using Finova.Core.Iban;

namespace Finova.Countries.Europe.Bulgaria.Models;

/// <summary>
/// Bulgaria specific IBAN details.
/// BG IBAN format: BG + 2 check + 4 Bank (BIC) + 4 Branch + 2 Type + 8 Account.
/// </summary>
public record BulgariaIbanDetails : IbanDetails
{
    /// <summary>
    /// Bankov Kod / BIC (Bank Code - 4 letters).
    /// Identifier of the bank.
    /// </summary>
    public required string BankovKod { get; init; }

    /// <summary>
    /// Klon (Branch / Account Type - 4 digits).
    /// Often represents the branch identifier.
    /// </summary>
    public required string Klon { get; init; }

    /// <summary>
    /// Vid Smetka (Account Type - 2 digits).
    /// </summary>
    public required string VidSmetka { get; init; }

    /// <summary>
    /// Nomer Smetka (Account Number - 8 digits).
    /// The specific account ID.
    /// </summary>
    public required string NomerSmetka { get; init; }
}
