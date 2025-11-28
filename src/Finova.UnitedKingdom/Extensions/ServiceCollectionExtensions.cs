using Finova.Core.Interfaces;
using Finova.UnitedKingdom.Services;
using Finova.UnitedKingdom.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Finova.UnitedKingdom.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers UnitedKingdom banking and validation services into the .NET DI container.
        /// </summary>
        public static IServiceCollection AddFinovaUnitedKingdom(this IServiceCollection services)
        {
            // IBAN validation - Register concrete class and interface
            services.AddSingleton<UnitedKingdomIbanValidator>();
            services.AddSingleton<IIbanValidator>(sp => sp.GetRequiredService<UnitedKingdomIbanValidator>());

            // IBAN parsing - Depends on UnitedKingdomIbanValidator
            services.AddSingleton<UnitedKingdomIbanParser>();
            services.AddSingleton<IIbanParser>(sp => sp.GetRequiredService<UnitedKingdomIbanParser>());

            return services;
        }
    }
}
