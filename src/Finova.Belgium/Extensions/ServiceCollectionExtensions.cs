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
        public static IServiceCollection AddBelgianPaymentReference(this IServiceCollection services)
        {
            // Payment reference generation and validation
            services.AddSingleton<IPaymentReferenceGenerator, BelgianPaymentReferenceService>();

            // IBAN validation
            services.AddSingleton<IBankAccountValidator, BelgianBankAccountValidator>();

            return services;
        }
    }
}
