using Microsoft.Extensions.DependencyInjection;
using Finova.Core.Identifiers;
using Finova.Services;

namespace Finova.Extensions.DependencyInjection;

public static class NorthAmericaServiceCollectionExtensions
{
    /// <summary>
    /// Registers North American financial validators (USA, Canada).
    /// </summary>
    public static IServiceCollection AddFinovaNorthAmerica(this IServiceCollection services)
    {
        services.AddSingleton<NorthAmericaBankValidator>();

        services.RegisterValidatorsFromNamespace(
            typeof(NorthAmericaServiceCollectionExtensions).Assembly,
            "Finova.Countries.NorthAmerica",
            null, // No special adapters needed beyond standard interfaces
            typeof(ITaxIdValidator),
            typeof(INationalIdValidator),
            typeof(IBankRoutingValidator),
            typeof(IBankRoutingParser)
        );

        return services;
    }
}
