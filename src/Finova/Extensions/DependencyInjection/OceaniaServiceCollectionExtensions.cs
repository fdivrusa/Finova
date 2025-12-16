using Microsoft.Extensions.DependencyInjection;
using Finova.Core.Identifiers;
using Finova.Countries.Oceania.Australia.Validators;

namespace Finova.Extensions.DependencyInjection;

public static class OceaniaServiceCollectionExtensions
{
    /// <summary>
    /// Registers Oceanian financial validators (Australia).
    /// </summary>
    public static IServiceCollection AddFinovaOceania(this IServiceCollection services)
    {
        AddAustraliaValidators(services);
        return services;
    }

    private static void AddAustraliaValidators(IServiceCollection services)
    {
        services.AddSingleton<ITaxIdValidator, AustraliaTfnValidator>();
        services.AddSingleton<AustraliaTfnValidator>();
        services.AddSingleton<ITaxIdValidator, AustraliaAbnValidator>(); // ABN is a business number, often treated as tax/business ID.
        services.AddSingleton<AustraliaAbnValidator>();
        services.AddSingleton<IBankRoutingValidator, AustraliaBsbValidator>();
        services.AddSingleton<AustraliaBsbValidator>();
    }
}
