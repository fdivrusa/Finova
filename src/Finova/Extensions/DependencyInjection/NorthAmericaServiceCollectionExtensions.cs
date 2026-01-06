using System.Reflection;
using Finova.Core.Iban;
using Finova.Core.Identifiers;
using Finova.Services;
using Finova.Services.Adapters;
using Microsoft.Extensions.DependencyInjection;

namespace Finova.Extensions.DependencyInjection;

public static class NorthAmericaServiceCollectionExtensions
{
    /// <summary>
    /// Registers North American financial validators (USA, Canada, Costa Rica, Dominican Republic, El Salvador, Guatemala, Virgin Islands British).
    /// </summary>
    public static IServiceCollection AddFinovaNorthAmerica(this IServiceCollection services)
    {
        services.AddSingleton<NorthAmericaBankValidator>();

        var assembly = Assembly.GetAssembly(typeof(NorthAmericaServiceCollectionExtensions)) ?? Assembly.GetExecutingAssembly();

        services.RegisterValidatorsFromNamespace(
            assembly,
            "Finova.Countries.NorthAmerica",
            (s, type) =>
            {
                // Special handling for IBAN validators to register IBankAccountValidator adapter
                if (typeof(IIbanValidator).IsAssignableFrom(type))
                {
                    s.AddSingleton<IBankAccountValidator>(sp =>
                        new IbanBankAccountAdapter((IIbanValidator)sp.GetRequiredService(type)));
                }
            },
            typeof(ITaxIdValidator),
            typeof(INationalIdValidator),
            typeof(IBankRoutingValidator),
            typeof(IBankRoutingParser),
            typeof(IIbanValidator),
            typeof(IBbanValidator)
        );

        return services;
    }
}
