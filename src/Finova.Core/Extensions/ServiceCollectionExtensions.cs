using Microsoft.Extensions.DependencyInjection;
using Finova.Core.Iban;
using Finova.Core.Bic;
using Finova.Core.PaymentCard;
using Finova.Core.PaymentReference;

namespace Finova.Core.Extensions;

/// <summary>
/// Extension methods for IServiceCollection to register Finova Core services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers Finova Core services into the IServiceCollection.
    /// </summary>
    public static IServiceCollection AddFinovaCoreServices(this IServiceCollection services)
    {
        services.AddSingleton<IIbanService, IbanService>();
        services.AddSingleton<IIbanParser, IbanParser>();
        services.AddSingleton<IBicValidator, BicValidator>();
        services.AddSingleton<IPaymentCardValidator, PaymentCardValidator>();
        services.AddSingleton<IPaymentReferenceValidator, IsoPaymentReferenceValidator>();
        services.AddSingleton<IPaymentReferenceGenerator, IsoPaymentReferenceGenerator>();

        return services;
    }
}
