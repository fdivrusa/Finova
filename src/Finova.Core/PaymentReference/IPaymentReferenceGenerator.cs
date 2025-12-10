namespace Finova.Core.PaymentReference;

public interface IPaymentReferenceGenerator
{
    string CountryCode { get; }

    /// <summary>
    /// Generates a valid payment reference for the specific country.
    /// </summary>
    /// <param name="rawReference">The invoice or customer ID.</param>
    /// <param name="format">The desired format (defaults to Domestic).</param>
    string Generate(string rawReference, PaymentReferenceFormat format = PaymentReferenceFormat.IsoRf);
}
