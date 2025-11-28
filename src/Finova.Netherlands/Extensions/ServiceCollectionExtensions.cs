using Finova.Core.Interfaces;
using Finova.Netherlands.Services;
using Finova.Netherlands.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Finova.Netherlands.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers Netherlands banking and validation services into the .NET DI container.
        /// </summary>
        public static IServiceCollection AddFinovaNetherlands(this IServiceCollection services)
        {
            // IBAN validation - Register concrete class and interface
            services.AddSingleton<NetherlandsIbanValidator>();
            services.AddSingleton<IIbanValidator>(sp => sp.GetRequiredService<NetherlandsIbanValidator>());

            // IBAN parsing - Depends on NetherlandsIbanValidator
            services.AddSingleton<NetherlandsIbanParser>();
            services.AddSingleton<IIbanParser>(sp => sp.GetRequiredService<NetherlandsIbanParser>());

            return services;
        }
    }
}
