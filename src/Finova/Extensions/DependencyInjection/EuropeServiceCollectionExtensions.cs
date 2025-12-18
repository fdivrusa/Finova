using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Finova.Core.Iban;
using Finova.Core.Vat;
using Finova.Services.Adapters;
using Finova.Core.Identifiers;
using Finova.Services;

namespace Finova.Extensions.DependencyInjection;

public static class EuropeServiceCollectionExtensions
{
    /// <summary>
    /// Registers European financial validators (IBAN, VAT, TaxID, NationalID).
    /// </summary>
    public static IServiceCollection AddFinovaEurope(this IServiceCollection services)
    {
        // Register continent-level services
        services.AddSingleton<IIbanParser, EuropeIbanParser>();
        services.AddSingleton<IIbanValidator, EuropeIbanValidator>();
        services.AddSingleton<IVatValidator, EuropeVatValidator>();
        services.AddSingleton<EuropeBankValidator>();

        // Auto-register all validators under the Finova.Countries.Europe namespace
        var assembly = Assembly.GetAssembly(typeof(EuropeServiceCollectionExtensions)) ?? Assembly.GetExecutingAssembly();
        
        services.RegisterValidatorsFromNamespace(
            assembly, 
            "Finova.Countries.Europe",
            (s, type) => 
            {
                // Special handling for IBAN validators to register IBankAccountValidator adapter
                if (typeof(IIbanValidator).IsAssignableFrom(type))
                {
                    s.AddSingleton<IBankAccountValidator>(sp => 
                        new EuropeIbanBankAccountAdapter((IIbanValidator)sp.GetRequiredService(type)));
                }
                
                // Special handling for VAT validators to register ITaxIdValidator adapter
                if (typeof(IVatValidator).IsAssignableFrom(type))
                {
                    s.AddSingleton<ITaxIdValidator>(sp => 
                        new EuropeVatTaxIdAdapter((IVatValidator)sp.GetRequiredService(type)));
                }
            },
            typeof(IVatValidator),
            typeof(ITaxIdValidator),
            typeof(INationalIdValidator),
            typeof(IIbanValidator),
            typeof(IBankRoutingValidator),
            typeof(IBbanValidator)
        );

        return services;
    }
}
