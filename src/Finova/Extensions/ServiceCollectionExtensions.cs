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
using Finova.Core.Identifiers;
using Finova.Services.Global;

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
        services.AddSingleton<IPaymentReferenceValidator, PaymentReferenceValidator>();
        services.AddSingleton<IPaymentReferenceGenerator, PaymentReferenceGenerator>();
        services.AddSingleton<IBicValidator, BicValidator>();
        services.AddSingleton<IPaymentCardValidator, PaymentCardValidator>();

        // Register global composite services
        services.AddSingleton<ITaxIdService, TaxIdService>();
        services.AddSingleton<INationalIdService, NationalIdService>();
        services.AddSingleton<IBankRoutingService, BankRoutingService>();
        services.AddSingleton<IBankAccountService, BankAccountService>();
        services.AddSingleton<BatchValidationService>();

        // Register continent-specific validators
        services.AddFinovaEurope();
        services.AddFinovaNorthAmerica();
        services.AddFinovaSouthAmerica();
        services.AddFinovaAsia();
        services.AddFinovaOceania();

        return services;
    }
}
