using Finova.Belgium.Services;
using Finova.Core.Interfaces;
using Finova.Core.Validators;
using Finova.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Finova.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers Finova financial services (IBAN, BIC, Cards) into the Dependency Injection container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddFinova(this IServiceCollection services)
    {
        services.AddSingleton<IBicValidator, BicValidator>();
        services.AddSingleton<IPaymentCardValidator, PaymentCardValidator>();
        services.AddSingleton<IPaymentReferenceValidator, PaymentReferenceValidator>();

        services.AddSingleton<IPaymentReferenceGenerator, BelgiumPaymentReferenceService>();

        services.AddSingleton<IIbanParser, EuropeIbanParser>();
        services.AddSingleton<IIbanValidator, EuropeIbanValidator>();

        return services;
    }
}
