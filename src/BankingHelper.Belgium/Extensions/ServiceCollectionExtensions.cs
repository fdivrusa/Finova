using BankingHelper.Belgium.Services;
using BankingHelper.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BankingHelper.Belgium.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers Belgian banking services into the .NET DI container.
        /// </summary>
        public static IServiceCollection AddBelgianBanking(this IServiceCollection services)
        {
            // OGM and ISO 11649 logic are combined here, registered under the Core interface.
            services.AddSingleton<IPaymentReferenceGenerator, BelgianPaymentService>();

            // Add other Belgian services later (IBAN Validator, etc.)
            // services.AddSingleton<IBankAccountValidator, BelgianIbanValidator>();

            return services;
        }
    }
}
