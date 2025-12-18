using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Finova.Core.Identifiers;
using Finova.Services;

namespace Finova.Extensions.DependencyInjection;

public static class AsiaServiceCollectionExtensions
{
    /// <summary>
    /// Registers Asian financial validators (China, India, Japan, Singapore).
    /// </summary>
    public static IServiceCollection AddFinovaAsia(this IServiceCollection services)
    {
        services.AddSingleton<AsiaBankValidator>();

        // Auto-register all validators under the Finova.Countries.Asia namespace
        var assembly = Assembly.GetAssembly(typeof(AsiaServiceCollectionExtensions)) ?? Assembly.GetExecutingAssembly();
        
        services.RegisterValidatorsFromNamespace(
            assembly, 
            "Finova.Countries.Asia",
            null, // No special adapters needed for Asia yet
            typeof(ITaxIdValidator),
            typeof(INationalIdValidator),
            typeof(IBankRoutingValidator),
            typeof(IBankAccountValidator)
        );

        return services;
    }
}
