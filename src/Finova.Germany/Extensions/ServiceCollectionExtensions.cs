using Finova.Core.Interfaces;
using Finova.Germany.Services;
using Finova.Germany.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Finova.Germany.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers Germany banking and validation services into the .NET DI container.
        /// </summary>
        public static IServiceCollection AddFinovaGermany(this IServiceCollection services)
        {
            // IBAN validation - Register concrete class and interface
            services.AddSingleton<GermanyIbanValidator>();
            services.AddSingleton<IIbanValidator>(sp => sp.GetRequiredService<GermanyIbanValidator>());

            // IBAN parsing - Depends on GermanyIbanValidator
            services.AddSingleton<GermanyIbanParser>();
            services.AddSingleton<IIbanParser>(sp => sp.GetRequiredService<GermanyIbanParser>());

            return services;
        }
    }
}
