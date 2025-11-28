using Finova.Belgium.Services;
using Finova.Belgium.Validators;
using Finova.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Finova.Belgium.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers all Belgian banking and validation services into the .NET DI container.
        /// </summary>
        public static IServiceCollection AddFinovaBelgium(this IServiceCollection services)
        {
            // Payment reference generation and validation
            services.AddSingleton<BelgiumPaymentReferenceService>();
            services.AddSingleton<IPaymentReferenceGenerator>(sp => sp.GetRequiredService<BelgiumPaymentReferenceService>());

            // IBAN validation - Register concrete class and interface
            services.AddSingleton<BelgiumIbanValidator>();
            services.AddSingleton<IIbanValidator>(sp => sp.GetRequiredService<BelgiumIbanValidator>());

            // IBAN parsing - Depends on BelgiumIbanValidator
            services.AddSingleton<BelgiumIbanParser>();
            services.AddSingleton<IIbanParser>(sp => sp.GetRequiredService<BelgiumIbanParser>());

            return services;
        }
    }
}
