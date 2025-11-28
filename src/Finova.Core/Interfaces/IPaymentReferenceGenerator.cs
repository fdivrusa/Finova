using Finova.Core.Models;

namespace Finova.Core.Interfaces
{
    public interface IPaymentReferenceGenerator
    {
        string CountryCode { get; }

        /// <summary>
        /// Generates a valid payment reference for the specific country.
        /// </summary>
        /// <param name="rawReference">The invoice or customer ID.</param>
        /// <param name="format">The desired format (defaults to Domestic).</param>
        string Generate(string rawReference, PaymentReferenceFormat format = PaymentReferenceFormat.IsoRf);

        /// <summary>
        /// Attempts to validate the reference, regardless of its specific format.
        /// </summary>
        bool IsValid(string communication);
    }
}
