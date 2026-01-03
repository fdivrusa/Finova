using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Finova.Core.Identifiers;
using Finova.Core.Iban;
using Finova.Services;
using Finova.Services.Adapters;

namespace Finova.Extensions.DependencyInjection;

public static class AsiaServiceCollectionExtensions
{
    /// <summary>
    /// Registers Asian financial validators (China, India, Japan, Singapore, Kazakhstan, Pakistan, Timor-Leste).
    /// </summary>
    public static IServiceCollection AddFinovaAsia(this IServiceCollection services)
    {
        services.AddSingleton<AsiaBankValidator>();

        // Auto-register all validators under the Finova.Countries.Asia namespace
        var assembly = Assembly.GetAssembly(typeof(AsiaServiceCollectionExtensions)) ?? Assembly.GetExecutingAssembly();

        services.RegisterValidatorsFromNamespace(
            assembly,
            "Finova.Countries.Asia",
            (s, type) =>
            {
                // Special handling for IBAN validators to register IBankAccountValidator adapter
                if (typeof(IIbanValidator).IsAssignableFrom(type))
                {
                    s.AddSingleton<IBankAccountValidator>(sp =>
                        new EuropeIbanBankAccountAdapter((IIbanValidator)sp.GetRequiredService(type)));
                }
            },
            typeof(ITaxIdValidator),
            typeof(INationalIdValidator),
            typeof(IBankRoutingValidator),
            typeof(IBankAccountValidator),
            typeof(IIbanValidator)
        );

        return services;
    }
}
