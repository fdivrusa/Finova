using Microsoft.Extensions.DependencyInjection;
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
using Finova.Countries.Europe.FaroeIslands.Validators;
using Finova.Countries.Europe.Finland.Validators;
using Finova.Countries.Europe.France.Validators;
using Finova.Countries.Europe.Georgia.Validators;
using Finova.Countries.Europe.Germany.Validators;
using Finova.Countries.Europe.Greece.Validators;
using Finova.Countries.Europe.Hungary.Validators;
using Finova.Countries.Europe.Iceland.Validators;
using Finova.Countries.Europe.Ireland.Validators;
using Finova.Countries.Europe.Italy.Validators;
using Finova.Countries.Europe.Latvia.Validators;
using Finova.Countries.Europe.Lithuania.Validators;
using Finova.Countries.Europe.Luxembourg.Validators;
using Finova.Countries.Europe.Malta.Validators;
using Finova.Countries.Europe.Monaco.Validators;
using Finova.Countries.Europe.Netherlands.Validators;
using Finova.Countries.Europe.Norway.Validators;
using Finova.Countries.Europe.Poland.Validators;
using Finova.Countries.Europe.Portugal.Validators;
using Finova.Countries.Europe.Romania.Validators;
using Finova.Countries.Europe.SanMarino.Validators;
using Finova.Countries.Europe.Serbia.Validators;
using Finova.Countries.Europe.Slovakia.Validators;
using Finova.Countries.Europe.Slovenia.Validators;
using Finova.Countries.Europe.Spain.Validators;
using Finova.Countries.Europe.Sweden.Validators;
using Finova.Countries.Europe.Switzerland.Validators;
using Finova.Countries.Europe.Turkey.Validators;
using Finova.Countries.Europe.Ukraine.Validators;
using Finova.Countries.Europe.UnitedKingdom.Validators;

namespace Finova.Extensions.DependencyInjection;

public static class VatValidatorsExtensions
{
    public static IServiceCollection AddVatValidators(this IServiceCollection services)
    {
        services.AddSingleton<AlbaniaVatValidator>();
        services.AddSingleton<AndorraVatValidator>();
        services.AddSingleton<AustriaVatValidator>();
        services.AddSingleton<AzerbaijanVatValidator>();
        services.AddSingleton<BelarusVatValidator>();
        services.AddSingleton<BelgiumVatValidator>();
        services.AddSingleton<BosniaAndHerzegovinaVatValidator>();
        services.AddSingleton<BulgariaVatValidator>();
        services.AddSingleton<CroatiaVatValidator>();
        services.AddSingleton<CyprusVatValidator>();
        services.AddSingleton<CzechRepublicVatValidator>();
        services.AddSingleton<DenmarkVatValidator>();
        services.AddSingleton<EstoniaVatValidator>();
        services.AddSingleton<FaroeIslandsVatValidator>();
        services.AddSingleton<FinlandVatValidator>();
        services.AddSingleton<FranceVatValidator>();
        services.AddSingleton<GeorgiaVatValidator>();
        services.AddSingleton<GermanyVatValidator>();
        services.AddSingleton<GreeceVatValidator>();
        services.AddSingleton<HungaryVatValidator>();
        services.AddSingleton<IcelandVatValidator>();
        services.AddSingleton<IrelandVatValidator>();
        services.AddSingleton<ItalyVatValidator>();
        services.AddSingleton<LatviaVatValidator>();
        services.AddSingleton<LithuaniaVatValidator>();
        services.AddSingleton<LuxembourgVatValidator>();
        services.AddSingleton<MaltaVatValidator>();
        services.AddSingleton<MonacoVatValidator>();
        services.AddSingleton<NetherlandsVatValidator>();
        services.AddSingleton<NorwayVatValidator>();
        services.AddSingleton<PolandVatValidator>();
        services.AddSingleton<PortugalVatValidator>();
        services.AddSingleton<RomaniaVatValidator>();
        services.AddSingleton<SanMarinoVatValidator>();
        services.AddSingleton<SerbiaVatValidator>();
        services.AddSingleton<SlovakiaVatValidator>();
        services.AddSingleton<SloveniaVatValidator>();
        services.AddSingleton<SpainVatValidator>();
        services.AddSingleton<SwedenVatValidator>();
        services.AddSingleton<SwitzerlandVatValidator>();
        services.AddSingleton<TurkeyVatValidator>();
        services.AddSingleton<UkraineVatValidator>();
        services.AddSingleton<UnitedKingdomVatValidator>();

        return services;
    }
}
