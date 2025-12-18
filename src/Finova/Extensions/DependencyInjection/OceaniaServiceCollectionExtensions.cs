using Microsoft.Extensions.DependencyInjection;
using Finova.Core.Identifiers;
using Finova.Services;

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
            typeof(IBankAccountValidator)
        );

        return services;
    }
}
