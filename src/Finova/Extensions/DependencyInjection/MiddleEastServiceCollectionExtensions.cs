using Microsoft.Extensions.DependencyInjection;
using Finova.Core.Identifiers;
using Finova.Services;
using Finova.Countries.MiddleEast.Israel.Validators;
using Finova.Countries.MiddleEast.UAE.Validators;
using Finova.Countries.MiddleEast.SaudiArabia.Validators;

namespace Finova.Extensions.DependencyInjection;

public static class MiddleEastServiceCollectionExtensions
{
    /// <summary>
    /// Registers Middle East financial validators (Israel, UAE, Saudi Arabia).
    /// </summary>
    public static IServiceCollection AddFinovaMiddleEast(this IServiceCollection services)
    {
        services.RegisterValidatorsFromNamespace(
            typeof(MiddleEastServiceCollectionExtensions).Assembly,
            "Finova.Countries.MiddleEast",
            null,
            typeof(ITaxIdValidator),
            typeof(INationalIdValidator),
            typeof(IBankRoutingValidator)
        );

        services.AddSingleton<INationalIdValidator, IsraelTeudatZehutValidator>();
        services.AddSingleton<INationalIdValidator, UaeEmiratesIdValidator>();
        services.AddSingleton<INationalIdValidator, SaudiArabiaIdValidator>();

        return services;
    }
}
