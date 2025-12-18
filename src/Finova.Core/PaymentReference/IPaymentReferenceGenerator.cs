namespace Finova.Core.PaymentReference;

public interface IPaymentReferenceGenerator
{
    string CountryCode { get; }

    /// <summary>
    /// Generates a valid payment reference for the specific country.
    /// </summary>
    /// <param name="rawReference">The invoice or customer ID.</param>
    /// <param name="format">The desired format (defaults to Domestic).</param>
    /// <returns>The generated payment reference string.</returns>
    /// <example>
    /// <code>
    /// var generator = new BelgiumPaymentReferenceService();
    /// string ogm = generator.Generate("1234567890", PaymentReferenceFormat.LocalBelgian);
    /// // Result: +++123/4567/89012+++
    /// </code>
    /// </example>
    string Generate(string rawReference, PaymentReferenceFormat format = PaymentReferenceFormat.IsoRf);

    /// <summary>
    /// Parses a payment reference and extracts details.
    /// </summary>
    /// <param name="reference">The payment reference string.</param>
    /// <returns>Details about the reference.</returns>
    /// <example>
    /// <code>
    /// var details = generator.Parse("+++123/4567/89012+++");
    /// Console.WriteLine(details.Content); // 1234567890
    /// </code>
    /// </example>
    PaymentReferenceDetails Parse(string reference);
}
