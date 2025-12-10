namespace Finova.Core.PaymentReference;

public enum PaymentReferenceFormat
{
    /// <summary>
    /// Specific format used in Belgium (ex: 123/4567/89012).
    /// </summary>
    LocalBelgian,

    /// <summary>
    /// ISO standard format (ex: RF18 1234 5678 9012).
    /// </summary>
    IsoRf,

    /// <summary>
    /// Specific format used in Finland (Viitenumero).
    /// </summary>
    LocalFinland,

    /// <summary>
    /// Specific format used in Norway (KID - Kundeidentifikasjonsnummer).
    /// </summary>
    LocalNorway,

    /// <summary>
    /// Specific format used in Sweden (OCR / Bankgiro Reference).
    /// </summary>
    LocalSweden,

    /// <summary>
    /// Specific format used in Switzerland (QR-Reference / ISR).
    /// </summary>
    LocalSwitzerland,

    /// <summary>
    /// Specific format used in Slovenia (SI12).
    /// </summary>
    LocalSlovenia
}
