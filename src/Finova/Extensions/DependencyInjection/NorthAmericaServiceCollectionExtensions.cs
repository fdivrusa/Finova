using Microsoft.Extensions.DependencyInjection;
using Finova.Core.Identifiers;
using Finova.Countries.NorthAmerica.UnitedStates.Validators;
using Finova.Countries.NorthAmerica.Canada.Validators;

namespace Finova.Extensions.DependencyInjection;

public static class NorthAmericaServiceCollectionExtensions
{
    /// <summary>
    /// Registers North American financial validators (USA, Canada).
    /// </summary>
    public static IServiceCollection AddFinovaNorthAmerica(this IServiceCollection services)
    {
        AddUnitedStatesValidators(services);
        AddCanadaValidators(services);
        return services;
    }

    private static void AddUnitedStatesValidators(IServiceCollection services)
    {
        services.AddSingleton<ITaxIdValidator, UnitedStatesEinValidator>();
        services.AddSingleton<UnitedStatesEinValidator>();
        
        // Register UnitedStatesRoutingNumberValidator as both Validator and Parser
        services.AddSingleton<UnitedStatesRoutingNumberValidator>();
        services.AddSingleton<IBankRoutingValidator>(sp => sp.GetRequiredService<UnitedStatesRoutingNumberValidator>());
        services.AddSingleton<IBankRoutingParser>(sp => sp.GetRequiredService<UnitedStatesRoutingNumberValidator>());
    }

    private static void AddCanadaValidators(IServiceCollection services)
    {
        services.AddSingleton<INationalIdValidator, CanadaSinValidator>();
        services.AddSingleton<CanadaSinValidator>();
        services.AddSingleton<ITaxIdValidator, CanadaBusinessNumberValidator>();
        services.AddSingleton<CanadaBusinessNumberValidator>();
        services.AddSingleton<IBankRoutingValidator, CanadaRoutingNumberValidator>();
        services.AddSingleton<CanadaRoutingNumberValidator>();
    }
}
