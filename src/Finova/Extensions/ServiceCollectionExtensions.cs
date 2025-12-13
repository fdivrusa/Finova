using Finova.Core.Iban;
using Finova.Core.Bic;
using Finova.Core.PaymentCard;
using Finova.Core.PaymentReference;
using Finova.Services;
using Microsoft.Extensions.DependencyInjection;
using Finova.Generators;
using Finova.Validators;
using Finova.Core.Vat;
using Finova.Extensions.DependencyInjection;

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
        services.AddSingleton<IPaymentReferenceGenerator, PaymentReferenceGenerator>();

        services.AddSingleton<IIbanParser, EuropeIbanParser>();
        services.AddSingleton<IIbanValidator, EuropeIbanValidator>();
        services.AddSingleton<IVatValidator, EuropeVatValidator>();

        // Register specific validators via extensions
        services.AddEnterpriseValidators();
        services.AddIbanValidators();
        services.AddVatValidators();

        return services;
    }
}
