using Finova.Core.Interfaces;
using Finova.France.Services;
using Finova.France.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Finova.France.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers France banking and validation services into the .NET DI container.
        /// </summary>
        public static IServiceCollection AddFinovaFrance(this IServiceCollection services)
        {
            // IBAN validation - Register concrete class and interface
            services.AddSingleton<FranceIbanValidator>();
            services.AddSingleton<IIbanValidator>(sp => sp.GetRequiredService<FranceIbanValidator>());

            // IBAN parsing - Depends on FranceIbanValidator
            services.AddSingleton<FranceIbanParser>();
            services.AddSingleton<IIbanParser>(sp => sp.GetRequiredService<FranceIbanParser>());

            return services;
        }
    }
}
