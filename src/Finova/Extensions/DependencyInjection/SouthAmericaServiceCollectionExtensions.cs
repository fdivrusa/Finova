using Microsoft.Extensions.DependencyInjection;
using Finova.Core.Identifiers;
using Finova.Services;
using Finova.Countries.SouthAmerica.Colombia.Validators;

namespace Finova.Extensions.DependencyInjection;

public static class SouthAmericaServiceCollectionExtensions
{
    /// <summary>
    /// Registers South American financial validators (Brazil, Mexico, Colombia).
    /// </summary>
    public static IServiceCollection AddFinovaSouthAmerica(this IServiceCollection services)
    {
        services.RegisterValidatorsFromNamespace(
            typeof(SouthAmericaServiceCollectionExtensions).Assembly,
            "Finova.Countries.SouthAmerica",
            null,
            typeof(ITaxIdValidator),
            typeof(INationalIdValidator),
            typeof(IBankRoutingValidator)
        );

        services.AddSingleton<INationalIdValidator, ColombiaCedulaValidator>();

        return services;
    }
}
