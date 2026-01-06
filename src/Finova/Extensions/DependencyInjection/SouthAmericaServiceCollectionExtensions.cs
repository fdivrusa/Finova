using System.Reflection;
using Finova.Core.Iban;
using Finova.Core.Identifiers;
using Finova.Countries.SouthAmerica.Colombia.Validators;
using Finova.Services.Adapters;
using Microsoft.Extensions.DependencyInjection;

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
                    s.AddSingleton<IBankAccountValidator>(sp => new IbanBankAccountAdapter((IIbanValidator)sp.GetRequiredService(type)));
                }
            },
            typeof(ITaxIdValidator),
            typeof(INationalIdValidator),
            typeof(IBankRoutingValidator),
            typeof(IIbanValidator),
            typeof(IBbanValidator)
        );

        services.AddSingleton<INationalIdValidator, ColombiaCedulaValidator>();

        return services;
    }
}
