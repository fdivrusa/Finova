using Microsoft.Extensions.DependencyInjection;
using Finova.Core.Identifiers;
using Finova.Countries.Asia.China.Validators;
using Finova.Countries.Asia.India.Validators;
using Finova.Countries.Asia.Japan.Validators;
using Finova.Countries.Asia.Singapore.Validators;

namespace Finova.Extensions.DependencyInjection;

public static class AsiaServiceCollectionExtensions
{
    /// <summary>
    /// Registers Asian financial validators (China, India, Japan, Singapore).
    /// </summary>
    public static IServiceCollection AddFinovaAsia(this IServiceCollection services)
    {
        AddChinaValidators(services);
        AddIndiaValidators(services);
        AddJapanValidators(services);
        AddSingaporeValidators(services);
        return services;
    }

    private static void AddChinaValidators(IServiceCollection services)
    {
        services.AddSingleton<ITaxIdValidator, ChinaUnifiedSocialCreditCodeValidator>();
        services.AddSingleton<ChinaUnifiedSocialCreditCodeValidator>();
        services.AddSingleton<INationalIdValidator, ChinaResidentIdentityCardValidator>();
        services.AddSingleton<ChinaResidentIdentityCardValidator>();
        services.AddSingleton<IBankRoutingValidator, ChinaCnapsValidator>();
        services.AddSingleton<ChinaCnapsValidator>();
    }

    private static void AddIndiaValidators(IServiceCollection services)
    {
        services.AddSingleton<ITaxIdValidator, IndiaPanValidator>();
        services.AddSingleton<IndiaPanValidator>();
        services.AddSingleton<INationalIdValidator, IndiaAadhaarValidator>();
        services.AddSingleton<IndiaAadhaarValidator>();
        services.AddSingleton<IBankRoutingValidator, IndiaIfscValidator>();
        services.AddSingleton<IndiaIfscValidator>();
    }

    private static void AddJapanValidators(IServiceCollection services)
    {
        services.AddSingleton<IBankAccountValidator, JapanBankAccountValidator>();
        services.AddSingleton<JapanBankAccountValidator>();
    }

    private static void AddSingaporeValidators(IServiceCollection services)
    {
        services.AddSingleton<IBankAccountValidator, SingaporeBankAccountValidator>();
        services.AddSingleton<SingaporeBankAccountValidator>();
    }
}
