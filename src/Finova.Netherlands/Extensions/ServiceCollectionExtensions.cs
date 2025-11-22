using Finova.Core.Interfaces;
using Finova.Netherlands.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Finova.Netherlands.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers Dutch (Netherlands) banking and validation services into the .NET DI container.
        /// </summary>
        public static IServiceCollection AddDutchBanking(this IServiceCollection services)
        {
            // IBAN validation
            services.AddSingleton<IBankAccountValidator, DutchBankAccountValidator>();

            return services;
        }
    }
}
