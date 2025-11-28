using Finova.Core.Interfaces;
using Finova.Italy.Services;
using Finova.Italy.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Finova.Italy.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers Italy banking and validation services into the .NET DI container.
        /// </summary>
        public static IServiceCollection AddFinovaItaly(this IServiceCollection services)
        {
            // IBAN validation - Register concrete class and interface
            services.AddSingleton<ItalyIbanValidator>();
            services.AddSingleton<IIbanValidator>(sp => sp.GetRequiredService<ItalyIbanValidator>());

            // IBAN parsing - Depends on ItalyIbanValidator
            services.AddSingleton<ItalyIbanParser>();
            services.AddSingleton<IIbanParser>(sp => sp.GetRequiredService<ItalyIbanParser>());

            return services;
        }
    }
}
