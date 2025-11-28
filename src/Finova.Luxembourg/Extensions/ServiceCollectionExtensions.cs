using Finova.Core.Interfaces;
using Finova.Luxembourg.Services;
using Finova.Luxembourg.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Finova.Luxembourg.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers Luxembourg banking and validation services into the .NET DI container.
        /// </summary>
        public static IServiceCollection AddFinovaLuxembourg(this IServiceCollection services)
        {
            // IBAN validation - Register concrete class and interface
            services.AddSingleton<LuxembourgIbanValidator>();
            services.AddSingleton<IIbanValidator>(sp => sp.GetRequiredService<LuxembourgIbanValidator>());

            // IBAN parsing - Depends on LuxembourgIbanValidator
            services.AddSingleton<LuxembourgIbanParser>();
            services.AddSingleton<IIbanParser>(sp => sp.GetRequiredService<LuxembourgIbanParser>());

            return services;
        }
    }
}
