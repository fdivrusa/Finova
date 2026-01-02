using Finova.Countries.SoutheastAsia.Thailand.Validators;
using Finova.Countries.SoutheastAsia.Malaysia.Validators;
using Finova.Countries.SoutheastAsia.Indonesia.Validators;
using Finova.Countries.SoutheastAsia.Vietnam.Validators;
using Finova.Core.Identifiers;
using Microsoft.Extensions.DependencyInjection;

namespace Finova.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for setting up Southeast Asia validators in an <see cref="IServiceCollection" />.
/// </summary>
public static class SoutheastAsiaServiceCollectionExtensions
{
    /// <summary>
    /// Adds Southeast Asia validators to the specified <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <returns>The <see cref="IServiceCollection" /> so that additional calls can be chained.</returns>
    public static IServiceCollection AddFinovaSoutheastAsia(this IServiceCollection services)
    {
        services.AddSingleton<INationalIdValidator, ThailandIdValidator>();
        services.AddSingleton<INationalIdValidator, MalaysiaMyKadValidator>();
        services.AddSingleton<INationalIdValidator, IndonesiaNikValidator>();
        services.AddSingleton<INationalIdValidator, VietnamCitizenIdValidator>();
        return services;
    }
}
