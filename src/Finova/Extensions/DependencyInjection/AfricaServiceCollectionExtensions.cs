using Microsoft.Extensions.DependencyInjection;
using Finova.Core.Identifiers;
using Finova.Services;
using Finova.Countries.Africa.SouthAfrica.Validators;
using Finova.Countries.Africa.Egypt.Validators;
using Finova.Countries.Africa.Nigeria.Validators;
using Finova.Countries.Africa.Kenya.Validators;

namespace Finova.Extensions.DependencyInjection;

public static class AfricaServiceCollectionExtensions
{
    /// <summary>
    /// Registers African financial validators (South Africa, Nigeria, Kenya, Egypt).
    /// </summary>
    public static IServiceCollection AddFinovaAfrica(this IServiceCollection services)
    {
        services.RegisterValidatorsFromNamespace(
            typeof(AfricaServiceCollectionExtensions).Assembly,
            "Finova.Countries.Africa",
            null,
            typeof(ITaxIdValidator),
            typeof(INationalIdValidator),
            typeof(IBankRoutingValidator)
        );

        services.AddSingleton<INationalIdValidator, SouthAfricaIdValidator>();
        services.AddSingleton<INationalIdValidator, EgyptNationalIdValidator>();
        services.AddSingleton<INationalIdValidator, NigeriaNinValidator>();
        services.AddSingleton<INationalIdValidator, KenyaNationalIdValidator>();

        return services;
    }
}
