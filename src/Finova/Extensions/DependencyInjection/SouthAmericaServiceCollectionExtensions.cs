using Microsoft.Extensions.DependencyInjection;
using Finova.Core.Identifiers;
using Finova.Countries.SouthAmerica.Brazil.Validators;

namespace Finova.Extensions.DependencyInjection;

public static class SouthAmericaServiceCollectionExtensions
{
    /// <summary>
    /// Registers South American financial validators (Brazil).
    /// </summary>
    public static IServiceCollection AddFinovaSouthAmerica(this IServiceCollection services)
    {
        AddBrazilValidators(services);
        return services;
    }

    private static void AddBrazilValidators(IServiceCollection services)
    {
        services.AddSingleton<INationalIdValidator, BrazilCpfValidator>();
        services.AddSingleton<BrazilCpfValidator>();
        services.AddSingleton<ITaxIdValidator, BrazilCnpjValidator>();
        services.AddSingleton<BrazilCnpjValidator>();
        services.AddSingleton<IBankRoutingValidator, BrazilBankCodeValidator>();
        services.AddSingleton<BrazilBankCodeValidator>();
    }
}
