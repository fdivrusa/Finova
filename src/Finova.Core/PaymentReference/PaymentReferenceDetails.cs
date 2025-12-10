namespace Finova.Core.PaymentReference;

public record PaymentReferenceDetails
{
    /// <summary>
    /// The full formatted reference (e.g., "RF18 1234 5678").
    /// </summary>
    public required string Reference { get; init; }

    /// <summary>
    /// The raw business content without the "RF" prefix and check digits.
    /// </summary>
    public required string Content { get; init; }

    /// <summary>
    /// The format standard detected (e.g., IsoRf).
    /// </summary>
    public PaymentReferenceFormat Format { get; init; }

    public bool IsValid { get; init; }
}
