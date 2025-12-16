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
using Finova.Countries.Europe.Gibraltar.Validators;
using Finova.Countries.Europe.Greece.Validators;
using Finova.Countries.Europe.Greenland.Validators;
using Finova.Countries.Europe.Hungary.Validators;
using Finova.Countries.Europe.Iceland.Validators;
using Finova.Countries.Europe.Ireland.Validators;
using Finova.Countries.Europe.Italy.Validators;
using Finova.Countries.Europe.Kosovo.Validators;
using Finova.Countries.Europe.Latvia.Validators;
using Finova.Countries.Europe.Liechtenstein.Validators;
using Finova.Countries.Europe.Lithuania.Validators;
using Finova.Countries.Europe.Luxembourg.Validators;
using Finova.Countries.Europe.Malta.Validators;
using Finova.Countries.Europe.Moldova.Validators;
using Finova.Countries.Europe.Monaco.Validators;
using Finova.Countries.Europe.Montenegro.Validators;
using Finova.Countries.Europe.Netherlands.Validators;
using Finova.Countries.Europe.NorthMacedonia.Validators;
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
using Finova.Countries.Europe.Vatican.Validators;
using Finova.Core.Iban;
using Finova.Core.Vat;
using Finova.Services.Adapters;
using Finova.Core.Identifiers;
using Finova.Core.Enterprise;
using Finova.Services;

namespace Finova.Extensions.DependencyInjection;

public static class EuropeServiceCollectionExtensions
{
    /// <summary>
    /// Registers European financial validators (IBAN, VAT, Enterprise).
    /// </summary>
    public static IServiceCollection AddFinovaEurope(this IServiceCollection services)
    {
        services.AddSingleton<IIbanParser, EuropeIbanParser>();
        services.AddSingleton<IIbanValidator, EuropeIbanValidator>();
        services.AddSingleton<IVatValidator, EuropeVatValidator>();

        AddIbanValidators(services);
        AddVatValidators(services);
        AddEnterpriseValidators(services);
        return services;
    }

    private static void AddIbanValidator<T>(IServiceCollection services) where T : class, IIbanValidator
    {
        services.AddSingleton<T>();
        services.AddSingleton<IBankAccountValidator>(sp => new EuropeIbanBankAccountAdapter(sp.GetRequiredService<T>()));
    }

    private static void AddVatValidator<T>(IServiceCollection services) where T : class, IVatValidator
    {
        services.AddSingleton<T>();
        services.AddSingleton<ITaxIdValidator>(sp => new EuropeVatTaxIdAdapter(sp.GetRequiredService<T>()));
    }

    private static void AddEnterpriseValidator<T>(IServiceCollection services) where T : class, IEnterpriseValidator
    {
        services.AddSingleton<T>();
        services.AddSingleton<ITaxIdValidator>(sp => new EuropeEnterpriseTaxIdAdapter(sp.GetRequiredService<T>()));
    }

    private static void AddIbanValidators(IServiceCollection services)
    {
        AddIbanValidator<AlbaniaIbanValidator>(services);
        AddIbanValidator<AndorraIbanValidator>(services);
        AddIbanValidator<AustriaIbanValidator>(services);
        AddIbanValidator<AzerbaijanIbanValidator>(services);
        AddIbanValidator<BelarusIbanValidator>(services);
        AddIbanValidator<BelgiumIbanValidator>(services);
        AddIbanValidator<BosniaAndHerzegovinaIbanValidator>(services);
        AddIbanValidator<BulgariaIbanValidator>(services);
        AddIbanValidator<CroatiaIbanValidator>(services);
        AddIbanValidator<CyprusIbanValidator>(services);
        AddIbanValidator<CzechRepublicIbanValidator>(services);
        AddIbanValidator<DenmarkIbanValidator>(services);
        AddIbanValidator<EstoniaIbanValidator>(services);
        AddIbanValidator<FaroeIslandsIbanValidator>(services);
        AddIbanValidator<FinlandIbanValidator>(services);
        AddIbanValidator<FranceIbanValidator>(services);
        AddIbanValidator<GeorgiaIbanValidator>(services);
        AddIbanValidator<GermanyIbanValidator>(services);
        AddIbanValidator<GibraltarIbanValidator>(services);
        AddIbanValidator<GreeceIbanValidator>(services);
        AddIbanValidator<GreenlandIbanValidator>(services);
        AddIbanValidator<HungaryIbanValidator>(services);
        AddIbanValidator<IcelandIbanValidator>(services);
        AddIbanValidator<IrelandIbanValidator>(services);
        AddIbanValidator<ItalyIbanValidator>(services);
        AddIbanValidator<KosovoIbanValidator>(services);
        AddIbanValidator<LatviaIbanValidator>(services);
        AddIbanValidator<LiechtensteinIbanValidator>(services);
        AddIbanValidator<LithuaniaIbanValidator>(services);
        AddIbanValidator<LuxembourgIbanValidator>(services);
        AddIbanValidator<MaltaIbanValidator>(services);
        AddIbanValidator<MoldovaIbanValidator>(services);
        AddIbanValidator<MonacoIbanValidator>(services);
        AddIbanValidator<MontenegroIbanValidator>(services);
        AddIbanValidator<NetherlandsIbanValidator>(services);
        AddIbanValidator<NorthMacedoniaIbanValidator>(services);
        AddIbanValidator<NorwayIbanValidator>(services);
        AddIbanValidator<PolandIbanValidator>(services);
        AddIbanValidator<PortugalIbanValidator>(services);
        AddIbanValidator<RomaniaIbanValidator>(services);
        AddIbanValidator<SanMarinoIbanValidator>(services);
        AddIbanValidator<SerbiaIbanValidator>(services);
        AddIbanValidator<SlovakiaIbanValidator>(services);
        AddIbanValidator<SloveniaIbanValidator>(services);
        AddIbanValidator<SpainIbanValidator>(services);
        AddIbanValidator<SwedenIbanValidator>(services);
        AddIbanValidator<SwitzerlandIbanValidator>(services);
        AddIbanValidator<TurkeyIbanValidator>(services);
        AddIbanValidator<UkraineIbanValidator>(services);
        AddIbanValidator<UnitedKingdomIbanValidator>(services);
        AddIbanValidator<VaticanIbanValidator>(services);
    }

    private static void AddVatValidators(IServiceCollection services)
    {
        AddVatValidator<AlbaniaVatValidator>(services);
        AddVatValidator<AndorraVatValidator>(services);
        AddVatValidator<AustriaVatValidator>(services);
        AddVatValidator<AzerbaijanVatValidator>(services);
        AddVatValidator<BelarusVatValidator>(services);
        AddVatValidator<BelgiumVatValidator>(services);
        AddVatValidator<BosniaAndHerzegovinaVatValidator>(services);
        AddVatValidator<BulgariaVatValidator>(services);
        AddVatValidator<CroatiaVatValidator>(services);
        AddVatValidator<CyprusVatValidator>(services);
        AddVatValidator<CzechRepublicVatValidator>(services);
        AddVatValidator<DenmarkVatValidator>(services);
        AddVatValidator<EstoniaVatValidator>(services);
        AddVatValidator<FaroeIslandsVatValidator>(services);
        AddVatValidator<FinlandVatValidator>(services);
        AddVatValidator<FranceVatValidator>(services);
        AddVatValidator<GeorgiaVatValidator>(services);
        AddVatValidator<GermanyVatValidator>(services);
        AddVatValidator<GreeceVatValidator>(services);
        AddVatValidator<HungaryVatValidator>(services);
        AddVatValidator<IcelandVatValidator>(services);
        AddVatValidator<IrelandVatValidator>(services);
        AddVatValidator<ItalyVatValidator>(services);
        AddVatValidator<LatviaVatValidator>(services);
        AddVatValidator<LithuaniaVatValidator>(services);
        AddVatValidator<LuxembourgVatValidator>(services);
        AddVatValidator<MaltaVatValidator>(services);
        AddVatValidator<MonacoVatValidator>(services);
        AddVatValidator<NetherlandsVatValidator>(services);
        AddVatValidator<NorwayVatValidator>(services);
        AddVatValidator<PolandVatValidator>(services);
        AddVatValidator<PortugalVatValidator>(services);
        AddVatValidator<RomaniaVatValidator>(services);
        AddVatValidator<SanMarinoVatValidator>(services);
        AddVatValidator<SerbiaVatValidator>(services);
        AddVatValidator<SlovakiaVatValidator>(services);
        AddVatValidator<SloveniaVatValidator>(services);
        AddVatValidator<SpainVatValidator>(services);
        AddVatValidator<SwedenVatValidator>(services);
        AddVatValidator<SwitzerlandVatValidator>(services);
        AddVatValidator<TurkeyVknValidator>(services);
        AddVatValidator<UkraineVatValidator>(services);
        AddVatValidator<UnitedKingdomVatValidator>(services);
    }

    private static void AddEnterpriseValidators(IServiceCollection services)
    {
        AddEnterpriseValidator<AustriaFirmenbuchValidator>(services);
        AddEnterpriseValidator<BelgiumEnterpriseValidator>(services);
        AddEnterpriseValidator<FranceSirenValidator>(services);
        AddEnterpriseValidator<FranceSiretValidator>(services);
        AddEnterpriseValidator<GermanySteuernummerValidator>(services);
        AddEnterpriseValidator<GermanyHandelsregisternummerValidator>(services);
        AddEnterpriseValidator<ItalyCodiceFiscaleValidator>(services);
        AddEnterpriseValidator<SpainCifValidator>(services);
        AddEnterpriseValidator<UnitedKingdomCompanyNumberValidator>(services);
        AddEnterpriseValidator<SwitzerlandUidValidator>(services);
        AddEnterpriseValidator<PortugalNifValidator>(services);
        AddEnterpriseValidator<IrelandVatNumberValidator>(services);
        AddEnterpriseValidator<IcelandKennitalaValidator>(services);
        AddEnterpriseValidator<AlbaniaNiptValidator>(services);
        AddEnterpriseValidator<AndorraNrtValidator>(services);
        AddEnterpriseValidator<AzerbaijanVoenValidator>(services);
        AddEnterpriseValidator<BelarusUnpValidator>(services);
        AddEnterpriseValidator<BosniaJibValidator>(services);
        AddEnterpriseValidator<BulgariaUicValidator>(services);
        AddEnterpriseValidator<CroatiaOibValidator>(services);
        AddEnterpriseValidator<CyprusTicValidator>(services);
        AddEnterpriseValidator<CzechRepublicIcoValidator>(services);
        AddEnterpriseValidator<DenmarkCvrValidator>(services);
        AddEnterpriseValidator<EstoniaRegistrikoodValidator>(services);
    }
}
