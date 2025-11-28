using Finova.Core.Interfaces;
using Finova.Core.Services;
using Finova.Core.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Finova.Core.Extensions
{
    /// <summary>
    /// Extension methods for IServiceCollection to register Finova Core services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers Finova Core services into the IServiceCollection.
        /// </summary>
        public static IServiceCollection AddFinovaCoreServices(this IServiceCollection services)
        {
            // Register Finova Core services here
            services.AddSingleton<IIbanService, IbanService>();
            services.AddSingleton<IIbanParser, IbanParser>();
            services.AddSingleton<IBicValidator, BicValidator>();
            services.AddSingleton<IPaymentCardValidator, PaymentCardValidator>();
            services.AddSingleton<IPaymentReferenceValidator, PaymentReferenceValidator>();

            // Add other service registrations as needed
            return services;
        }
    }
}
