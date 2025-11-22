using Finova.Core.Interfaces;
using Finova.Luxembourg.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Finova.Luxembourg.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers Luxembourg banking and validation services into the .NET DI container.
        /// </summary>
        public static IServiceCollection AddLuxembourgBanking(this IServiceCollection services)
        {
            // IBAN validation
            services.AddSingleton<IBankAccountValidator, LuxembourgBankAccountValidator>();

            return services;
        }
    }
}
