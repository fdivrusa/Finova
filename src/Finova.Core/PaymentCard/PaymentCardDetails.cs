namespace Finova.Core.PaymentCard;

/// <summary>
/// Represents the details of a parsed payment card number.
/// </summary>
public record PaymentCardDetails
{
    /// <summary>
    /// The card number (normalized, no spaces or dashes).
    /// </summary>
    public string CardNumber { get; init; } = string.Empty;

    /// <summary>
    /// The detected brand of the card.
    /// </summary>
    public PaymentCardBrand Brand { get; init; } = PaymentCardBrand.Unknown;

    /// <summary>
    /// Indicates if the card number is valid (Luhn check passed).
    /// </summary>
    public bool IsValid { get; init; }

    /// <summary>
    /// Indicates if the Luhn check passed.
    /// </summary>
    public bool IsLuhnValid { get; init; }
}
