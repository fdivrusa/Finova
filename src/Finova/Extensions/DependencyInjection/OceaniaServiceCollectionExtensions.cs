using Finova.Core.Iban;
using Finova.Core.Identifiers;
using Finova.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Finova.Extensions.DependencyInjection;

public static class OceaniaServiceCollectionExtensions
{
    /// <summary>
    /// Registers Oceanian financial validators (Australia).
    /// </summary>
    public static IServiceCollection AddFinovaOceania(this IServiceCollection services)
    {
        services.AddSingleton<OceaniaBankValidator>();

        services.RegisterValidatorsFromNamespace(
            typeof(OceaniaServiceCollectionExtensions).Assembly,
            "Finova.Countries.Oceania",
            null,
            typeof(ITaxIdValidator),
            typeof(INationalIdValidator),
            typeof(IBankRoutingValidator),
            typeof(IBankAccountValidator),
            typeof(IIbanValidator),
            typeof(IBbanValidator)
        );

        return services;
    }
}
