using Finova.Core.PaymentReference.Internals;

namespace Finova.Core.PaymentReference;

/// <summary>
/// Base generator for ISO 11649 (RF) payment references.
/// Can be inherited to support country-specific formats.
/// </summary>
public class IsoPaymentReferenceGenerator : IIsoPaymentReferenceGenerator
{

    /// <summary>
    /// Generates a formatted reference string based on the specified raw reference input.
    /// </summary>
    /// <param name="rawReference">The raw reference data to be formatted. Cannot be null or empty.</param>
    /// <returns>A string containing the formatted reference. The format of the output depends on the implementation and the
    /// content of the input.</returns>
    public string Generate(string rawReference)
    {
        return IsoReferenceHelper.Generate(rawReference);
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
