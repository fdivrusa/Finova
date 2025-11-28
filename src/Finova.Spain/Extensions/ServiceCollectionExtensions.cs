using Finova.Core.Interfaces;
using Finova.Spain.Services;
using Finova.Spain.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Finova.Spain.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers Spain banking and validation services into the .NET DI container.
        /// </summary>
        public static IServiceCollection AddFinovaSpain(this IServiceCollection services)
        {
            // IBAN validation - Register concrete class and interface
            services.AddSingleton<SpainIbanValidator>();
            services.AddSingleton<IIbanValidator>(sp => sp.GetRequiredService<SpainIbanValidator>());

            // IBAN parsing - Depends on SpainIbanValidator
            services.AddSingleton<SpainIbanParser>();
            services.AddSingleton<IIbanParser>(sp => sp.GetRequiredService<SpainIbanParser>());

            return services;
        }
    }
}
