using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Finova.Core.Identifiers;
using Finova.Core.Iban;
using Finova.Services;
using Finova.Services.Adapters;
using Finova.Countries.MiddleEast.Israel.Validators;
using Finova.Countries.MiddleEast.UAE.Validators;
using Finova.Countries.MiddleEast.SaudiArabia.Validators;

namespace Finova.Extensions.DependencyInjection;

public static class MiddleEastServiceCollectionExtensions
{
    /// <summary>
    /// Registers Middle East financial validators (Israel, UAE, Saudi Arabia, Bahrain, Jordan, Kuwait, Lebanon, Qatar).
    /// </summary>
    public static IServiceCollection AddFinovaMiddleEast(this IServiceCollection services)
    {
        var assembly = Assembly.GetAssembly(typeof(MiddleEastServiceCollectionExtensions)) ?? Assembly.GetExecutingAssembly();

        services.RegisterValidatorsFromNamespace(
            assembly,
            "Finova.Countries.MiddleEast",
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

        services.AddSingleton<INationalIdValidator, IsraelTeudatZehutValidator>();
        services.AddSingleton<INationalIdValidator, UaeEmiratesIdValidator>();
        services.AddSingleton<INationalIdValidator, SaudiArabiaIdValidator>();

        return services;
    }
}
