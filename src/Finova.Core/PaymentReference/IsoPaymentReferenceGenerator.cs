using Finova.Core.PaymentReference.Internals;

namespace Finova.Core.PaymentReference;

/// <summary>
/// Base generator for ISO 11649 (RF) payment references.
/// Can be inherited to support country-specific formats.
/// </summary>
public class IsoPaymentReferenceGenerator : IPaymentReferenceGenerator
{
    /// <summary>
    /// The country code this generator supports. 
    /// Defaults to "XX" (International/Generic) if not overridden.
    /// </summary>
    public virtual string CountryCode => "XX";

    /// <summary>
    /// Generates a payment reference.
    /// By default, supports <see cref="PaymentReferenceFormat.IsoRf"/>.
    /// Override to support additional formats.
    /// </summary>
    public virtual string Generate(string rawReference, PaymentReferenceFormat format = PaymentReferenceFormat.IsoRf)
    {
        if (format == PaymentReferenceFormat.IsoRf)
        {
            return IsoReferenceHelper.Generate(rawReference);
        }

        throw new NotSupportedException($"Format {format} is not supported by {CountryCode} generator.");
    }
}
