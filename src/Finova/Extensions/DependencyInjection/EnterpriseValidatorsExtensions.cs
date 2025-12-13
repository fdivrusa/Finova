using Microsoft.Extensions.DependencyInjection;
using Finova.Countries.Europe.Austria.Validators;
using Finova.Countries.Europe.Belgium.Validators;
using Finova.Countries.Europe.France.Validators;

namespace Finova.Extensions.DependencyInjection;

public static class EnterpriseValidatorsExtensions
{
    public static IServiceCollection AddEnterpriseValidators(this IServiceCollection services)
    {
        services.AddSingleton<AustriaFirmenbuchValidator>();
        services.AddSingleton<BelgiumEnterpriseValidator>();
        services.AddSingleton<FranceSirenValidator>();
        services.AddSingleton<FranceSiretValidator>();

        return services;
    }
}
