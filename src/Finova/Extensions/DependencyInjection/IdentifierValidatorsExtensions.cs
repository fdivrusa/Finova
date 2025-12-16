using Microsoft.Extensions.DependencyInjection;
using Finova.Core.Identifiers;
using Finova.Countries.NorthAmerica.UnitedStates.Validators;
using Finova.Countries.SouthAmerica.Brazil.Validators;
using Finova.Countries.Asia.China.Validators;
using Finova.Countries.Asia.India.Validators;
using Finova.Countries.NorthAmerica.Canada.Validators;
using Finova.Countries.Oceania.Australia.Validators;

namespace Finova.Extensions.DependencyInjection;

public static class IdentifierValidatorsExtensions
{
    public static IServiceCollection AddIdentifierValidators(this IServiceCollection services)
    {
        // United States
        services.AddSingleton<ITaxIdValidator, UnitedStatesEinValidator>();
        services.AddSingleton<UnitedStatesEinValidator>();
        services.AddSingleton<IBankRoutingValidator, UnitedStatesRoutingNumberValidator>();
        services.AddSingleton<UnitedStatesRoutingNumberValidator>();

        // Brazil
        services.AddSingleton<INationalIdValidator, BrazilCpfValidator>();
        services.AddSingleton<BrazilCpfValidator>();
        services.AddSingleton<ITaxIdValidator, BrazilCnpjValidator>();
        services.AddSingleton<BrazilCnpjValidator>();

        // China
        services.AddSingleton<ITaxIdValidator, ChinaUnifiedSocialCreditCodeValidator>();
        services.AddSingleton<ChinaUnifiedSocialCreditCodeValidator>();
        services.AddSingleton<INationalIdValidator, ChinaResidentIdentityCardValidator>();
        services.AddSingleton<ChinaResidentIdentityCardValidator>();

        // India
        services.AddSingleton<ITaxIdValidator, IndiaPanValidator>();
        services.AddSingleton<IndiaPanValidator>();
        services.AddSingleton<INationalIdValidator, IndiaAadhaarValidator>();
        services.AddSingleton<IndiaAadhaarValidator>();

        // Canada
        services.AddSingleton<INationalIdValidator, CanadaSinValidator>();
        services.AddSingleton<CanadaSinValidator>();
        services.AddSingleton<ITaxIdValidator, CanadaBusinessNumberValidator>();
        services.AddSingleton<CanadaBusinessNumberValidator>();

        // Australia
        services.AddSingleton<ITaxIdValidator, AustraliaTfnValidator>();
        services.AddSingleton<AustraliaTfnValidator>();
        services.AddSingleton<ITaxIdValidator, AustraliaAbnValidator>();
        services.AddSingleton<AustraliaAbnValidator>();

        return services;
    }
}
