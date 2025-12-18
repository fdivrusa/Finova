using Microsoft.Extensions.DependencyInjection;
using Finova.Core.Identifiers;
using Finova.Services;

namespace Finova.Extensions.DependencyInjection;

public static class SouthAmericaServiceCollectionExtensions
{
    /// <summary>
    /// Registers South American financial validators (Brazil, Mexico).
    /// </summary>
    public static IServiceCollection AddFinovaSouthAmerica(this IServiceCollection services)
    {
        services.AddSingleton<SouthAmericaBankValidator>();

        services.RegisterValidatorsFromNamespace(
            typeof(SouthAmericaServiceCollectionExtensions).Assembly,
            "Finova.Countries.SouthAmerica",
            null,
            typeof(ITaxIdValidator),
            typeof(INationalIdValidator),
            typeof(IBankRoutingValidator)
        );

        return services;
    }
}
