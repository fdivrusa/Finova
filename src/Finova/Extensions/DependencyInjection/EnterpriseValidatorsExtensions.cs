namespace Finova.Extensions.DependencyInjection;

using Finova.Countries.Europe.Albania.Validators;
using Finova.Countries.Europe.Andorra.Validators;
using Finova.Countries.Europe.Austria.Validators;
using Finova.Countries.Europe.Azerbaijan.Validators;
using Finova.Countries.Europe.Belarus.Validators;
using Finova.Countries.Europe.Belgium.Validators;
using Finova.Countries.Europe.BosniaAndHerzegovina.Validators;
using Finova.Countries.Europe.Bulgaria.Validators;
using Finova.Countries.Europe.Croatia.Validators;
using Finova.Countries.Europe.Cyprus.Validators;
using Finova.Countries.Europe.CzechRepublic.Validators;
using Finova.Countries.Europe.Denmark.Validators;
using Finova.Countries.Europe.Estonia.Validators;
using Finova.Countries.Europe.France.Validators;
using Finova.Countries.Europe.Germany.Validators;
using Finova.Countries.Europe.Iceland.Validators;
using Finova.Countries.Europe.Ireland.Validators;
using Finova.Countries.Europe.Italy.Validators;
using Finova.Countries.Europe.Portugal.Validators;
using Finova.Countries.Europe.Spain.Validators;
using Finova.Countries.Europe.Switzerland.Validators;
using Finova.Countries.Europe.UnitedKingdom.Validators;
using Microsoft.Extensions.DependencyInjection;

public static class EnterpriseValidatorsExtensions
{
    public static IServiceCollection AddEnterpriseValidators(this IServiceCollection services)
    {
        services.AddSingleton<AustriaFirmenbuchValidator>();
        services.AddSingleton<BelgiumEnterpriseValidator>();
        services.AddSingleton<FranceSirenValidator>();
        services.AddSingleton<FranceSiretValidator>();
        services.AddSingleton<GermanySteuernummerValidator>();
        services.AddSingleton<GermanyHandelsregisternummerValidator>();
        services.AddSingleton<ItalyCodiceFiscaleValidator>();
        services.AddSingleton<SpainCifValidator>();
        services.AddSingleton<UnitedKingdomCompanyNumberValidator>();
        services.AddSingleton<SwitzerlandUidValidator>();
        services.AddSingleton<PortugalNifValidator>();
        services.AddSingleton<IrelandVatNumberValidator>();
        services.AddSingleton<IcelandKennitalaValidator>();
        services.AddSingleton<AlbaniaNiptValidator>();
        services.AddSingleton<AndorraNrtValidator>();
        services.AddSingleton<AzerbaijanVoenValidator>();
        services.AddSingleton<BelarusUnpValidator>();
        services.AddSingleton<BosniaJibValidator>();
        services.AddSingleton<BulgariaUicValidator>();
        services.AddSingleton<CroatiaOibValidator>();
        services.AddSingleton<CyprusTicValidator>();
        services.AddSingleton<CzechRepublicIcoValidator>();
        services.AddSingleton<DenmarkCvrValidator>();
        services.AddSingleton<EstoniaRegistrikoodValidator>();

        return services;
    }
}
