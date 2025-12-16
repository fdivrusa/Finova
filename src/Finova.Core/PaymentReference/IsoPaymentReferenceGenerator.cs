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
        ArgumentNullException.ThrowIfNull(rawReference);

        if (format == PaymentReferenceFormat.IsoRf)
        {
            return IsoReferenceHelper.Generate(rawReference);
        }

        throw new NotSupportedException($"Format {format} is not supported by {CountryCode} generator.");
    }

    /// <summary>
    /// Parses a payment reference into its components.
    /// By default, supports <see cref="PaymentReferenceFormat.IsoRf"/>.
    /// Override to support additional formats or custom parsing logic.
    /// </summary>
    public virtual PaymentReferenceDetails Parse(string reference)
    {
        var validation = IsoReferenceValidator.Validate(reference);
        if (!validation.IsValid)
        {
            return new PaymentReferenceDetails
            {
                Reference = reference,
                Content = string.Empty,
                Format = PaymentReferenceFormat.Unknown,
                IsValid = false
            };
        }

        return new PaymentReferenceDetails
        {
            Reference = reference,
            Content = IsoReferenceHelper.Parse(reference),
            Format = PaymentReferenceFormat.IsoRf,
            IsValid = true
        };
    }
}
