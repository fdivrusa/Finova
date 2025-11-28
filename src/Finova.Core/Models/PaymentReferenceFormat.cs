namespace Finova.Core.Models
{
    public enum PaymentReferenceFormat
    {
        /// <summary>
        /// Specific format used in Belgium (ex: 123/4567/89012).
        /// </summary>
        LocalBelgian,

        /// <summary>
        /// ISO standard format (ex: RF18 1234 5678 9012).
        /// </summary>
        IsoRf
    }
}
