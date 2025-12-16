namespace Finova.Core.PaymentReference;

public interface IIsoPaymentReferenceGenerator
{
    /// <summary>
    /// Generates a valid payment reference
    /// </summary>
    /// <param name="rawReference">The invoice or customer ID.</param>
    /// <returns>The generated payment reference string.</returns>
    string Generate(string rawReference);

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
