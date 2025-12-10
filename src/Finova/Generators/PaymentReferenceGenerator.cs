using Finova.Core.PaymentReference;
using Finova.Belgium.Services;



using Finova.Countries.Europe.Finland.Services;
using Finova.Countries.Europe.Norway.Services;
using Finova.Countries.Europe.Slovenia.Services;
using Finova.Countries.Europe.Sweden.Services;
using Finova.Countries.Europe.Switzerland.Services;

using Finova.Core.PaymentReference.Internals;

namespace Finova.Generators;

/// <summary>
/// Composite payment reference generator that supports all payment reference formats.
/// Routes to country-specific implementations based on the requested format.
/// </summary>
public class PaymentReferenceGenerator : IsoPaymentReferenceGenerator
{
    /// <summary>
    /// Generates a payment reference in the specified format.
    /// </summary>
    /// <param name="rawReference">The raw reference data (e.g., invoice number, customer ID).</param>
    /// <param name="format">The desired payment reference format.</param>
    /// <returns>A valid payment reference in the specified format.</returns>
    public override string Generate(string rawReference, PaymentReferenceFormat format = PaymentReferenceFormat.IsoRf) => format switch
    {
        PaymentReferenceFormat.IsoRf => IsoReferenceHelper.Generate(rawReference),
        PaymentReferenceFormat.LocalBelgian => BelgiumPaymentReferenceService.GenerateOgmStatic(rawReference),
        PaymentReferenceFormat.LocalFinland => FinlandPaymentReferenceService.GenerateStatic(rawReference),
        PaymentReferenceFormat.LocalNorway => NorwayPaymentReferenceService.GenerateStatic(rawReference),
        PaymentReferenceFormat.LocalSweden => SwedenPaymentReferenceService.GenerateStatic(rawReference),
        PaymentReferenceFormat.LocalSwitzerland => SwitzerlandPaymentReferenceService.GenerateStatic(rawReference),
        PaymentReferenceFormat.LocalSlovenia => SloveniaPaymentReferenceService.GenerateStatic(rawReference),
        _ => throw new NotSupportedException($"Payment reference format {format} is not supported.")
    };
}

