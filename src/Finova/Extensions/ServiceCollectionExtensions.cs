using Finova.Core.Bic;
using Finova.Core.Enterprise;
using Finova.Core.Identifiers;
using Finova.Core.PaymentCard;
using Finova.Core.PaymentReference;
using Finova.Extensions.DependencyInjection;
using Finova.Generators;
using Finova.Services;
using Finova.Services.Global;
using Finova.Validators;
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
        services.AddSingleton<IPaymentReferenceValidator, PaymentReferenceValidator>();
        services.AddSingleton<IPaymentReferenceGenerator, PaymentReferenceGenerator>();
        services.AddSingleton<IBicValidator, BicValidator>();
        services.AddSingleton<IPaymentCardValidator, PaymentCardValidator>();

        // Register global composite services
        services.AddSingleton<ITaxIdService, TaxIdService>();
        services.AddSingleton<INationalIdService, NationalIdService>();
        services.AddSingleton<IBankRoutingService, BankRoutingService>();
        services.AddSingleton<IBankAccountService, BankAccountService>();
        services.AddSingleton<IBbanService, BbanService>();
        services.AddSingleton<BatchValidationService>();

        // Register Global Regional Routers
        services.AddSingleton<GlobalVatValidator>();
        services.AddSingleton<GlobalEnterpriseValidator>();
        services.AddSingleton<IGlobalEnterpriseValidator, GlobalEnterpriseValidator>();

        // Register continent-specific validators
        services.AddFinovaEurope();
        services.AddFinovaNorthAmerica();
        services.AddFinovaSouthAmerica();
        services.AddFinovaAsia();
        services.AddFinovaOceania();
        services.AddFinovaMiddleEast();
        services.AddFinovaAfrica();
        services.AddFinovaSoutheastAsia();

        return services;
    }
}
