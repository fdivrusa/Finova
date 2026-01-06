using System.Reflection;
using Finova.Core.Iban;
using Finova.Core.Identifiers;
using Finova.Countries.Africa.Egypt.Validators;
using Finova.Countries.Africa.Kenya.Validators;
using Finova.Countries.Africa.Nigeria.Validators;
using Finova.Countries.Africa.SouthAfrica.Validators;
using Finova.Services.Adapters;
using Microsoft.Extensions.DependencyInjection;

namespace Finova.Extensions.DependencyInjection;

public static class AfricaServiceCollectionExtensions
{
    /// <summary>
    /// Registers African financial validators (South Africa, Nigeria, Kenya, Egypt, Mauritania).
    /// </summary>
    public static IServiceCollection AddFinovaAfrica(this IServiceCollection services)
    {
        var assembly = Assembly.GetAssembly(typeof(AfricaServiceCollectionExtensions)) ?? Assembly.GetExecutingAssembly();

        services.RegisterValidatorsFromNamespace(
            assembly,
            "Finova.Countries.Africa",
            (s, type) =>
            {
                // Special handling for IBAN validators to register IBankAccountValidator adapter
                if (typeof(IIbanValidator).IsAssignableFrom(type))
                {
                    s.AddSingleton<IBankAccountValidator>(sp =>
                        new IbanBankAccountAdapter((IIbanValidator)sp.GetRequiredService(type)));
                }
            },
            typeof(ITaxIdValidator),
            typeof(INationalIdValidator),
            typeof(IBankRoutingValidator),
            typeof(IIbanValidator),
            typeof(IBbanValidator)
        );

        services.AddSingleton<INationalIdValidator, SouthAfricaIdValidator>();
        services.AddSingleton<INationalIdValidator, EgyptNationalIdValidator>();
        services.AddSingleton<INationalIdValidator, NigeriaNinValidator>();
        services.AddSingleton<INationalIdValidator, KenyaNationalIdValidator>();

        return services;
    }
}
