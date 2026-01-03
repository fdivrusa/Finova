using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Finova.Core.Identifiers;
using Finova.Core.Iban;
using Finova.Services;
using Finova.Services.Adapters;
using Finova.Countries.SouthAmerica.Colombia.Validators;

namespace Finova.Extensions.DependencyInjection;

public static class SouthAmericaServiceCollectionExtensions
{
    /// <summary>
    /// Registers South American financial validators (Brazil, Colombia).
    /// </summary>
    public static IServiceCollection AddFinovaSouthAmerica(this IServiceCollection services)
    {
        var assembly = Assembly.GetAssembly(typeof(SouthAmericaServiceCollectionExtensions)) ?? Assembly.GetExecutingAssembly();

        services.RegisterValidatorsFromNamespace(
            assembly,
            "Finova.Countries.SouthAmerica",
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
            typeof(IIbanValidator)
        );

        services.AddSingleton<INationalIdValidator, ColombiaCedulaValidator>();

        return services;
    }
}
