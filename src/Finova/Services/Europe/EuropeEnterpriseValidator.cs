using Finova.Core.Common;
using Finova.Core.Enterprise;
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
using Finova.Countries.Europe.Spain.Validators;
using Finova.Countries.Europe.Switzerland.Validators;
using Finova.Countries.Europe.UnitedKingdom.Validators;

using Finova.Countries.Europe.Slovenia.Validators;
using Finova.Countries.Europe.Sweden.Validators;
using Finova.Countries.Europe.Turkey.Validators;
using Finova.Countries.Europe.Ukraine.Validators;
using Finova.Countries.Europe.Vatican.Validators;


namespace Finova.Services;

/// <summary>
/// Unified validator for European Enterprise/Business Registration Numbers.
/// Delegates validation to specific country validators based on the country code or enterprise number type.
/// </summary>
/// <example>
/// <code>
/// // Validate by Country Code
/// var result = EuropeEnterpriseValidator.ValidateEnterpriseNumber("0403019261", "BE");
///
/// // Validate by Type
/// var result = EuropeEnterpriseValidator.ValidateEnterpriseNumber("123456789", EnterpriseNumberType.FranceSiren);
/// </code>
/// </example>
public class EuropeEnterpriseValidator : IEuropeEnterpriseValidator
{
    public ValidationResult Validate(string? number, string countryCode) => ValidateEnterpriseNumber(number, countryCode);

    public ValidationResult Validate(string? number, EnterpriseNumberType type) => ValidateEnterpriseNumber(number, type);

    public static string? Parse(string? number, string countryCode) => GetNormalizedNumber(number, countryCode);

    public static string? Parse(string? number, EnterpriseNumberType type) => GetNormalizedNumber(number, type);

    public static ValidationResult ValidateEnterpriseNumber(string? number, EnterpriseNumberType type)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        return type switch
        {
            EnterpriseNumberType.AlbaniaNipt => AlbaniaNiptValidator.ValidateNipt(number),
            EnterpriseNumberType.AndorraNrt => AndorraNrtValidator.ValidateNrt(number),
            EnterpriseNumberType.AustriaFirmenbuch => AustriaFirmenbuchValidator.ValidateFirmenbuch(number),
            EnterpriseNumberType.AzerbaijanVoen => AzerbaijanVoenValidator.ValidateVoen(number),
            EnterpriseNumberType.BelarusUnp => BelarusUnpValidator.ValidateUnp(number),
            EnterpriseNumberType.BelgiumEnterpriseNumber => BelgiumEnterpriseValidator.ValidateEnterpriseNumber(number),
            EnterpriseNumberType.BosniaJib => BosniaJibValidator.ValidateJib(number),
            EnterpriseNumberType.BulgariaUic => BulgariaUicValidator.ValidateUic(number),
            EnterpriseNumberType.CroatiaOib => CroatiaOibValidator.ValidateOib(number),
            EnterpriseNumberType.CyprusTic => CyprusTicValidator.ValidateTic(number),
            EnterpriseNumberType.CzechRepublicIco => CzechRepublicIcoValidator.ValidateIco(number),
            EnterpriseNumberType.DenmarkCvr => DenmarkCvrValidator.ValidateCvr(number),
            EnterpriseNumberType.EstoniaRegistrikood => EstoniaRegistrikoodValidator.ValidateRegistrikood(number),
            EnterpriseNumberType.FaroeIslandsVtal => FaroeIslandsVtalValidator.ValidateVtal(number),
            EnterpriseNumberType.FinlandBusinessId => FinlandBusinessIdValidator.ValidateBusinessId(number),
            EnterpriseNumberType.FranceSiren => FranceSirenValidator.ValidateSiren(number),
            EnterpriseNumberType.FranceSiret => FranceSiretValidator.ValidateSiret(number),
            EnterpriseNumberType.GeorgiaTaxId => GeorgiaTaxIdValidator.ValidateTaxId(number),
            EnterpriseNumberType.GermanySteuernummer => GermanySteuernummerValidator.ValidateSteuernummer(number),
            EnterpriseNumberType.GermanyHandelsregisternummer => GermanyHandelsregisternummerValidator.ValidateHandelsregisternummer(number),
            EnterpriseNumberType.GibraltarCompanyNumber => GibraltarCompanyNumberValidator.ValidateCompanyNumber(number),
            EnterpriseNumberType.GreeceAfm => GreeceAfmValidator.ValidateAfm(number),
            EnterpriseNumberType.GreenlandCvr => GreenlandCvrValidator.ValidateCvr(number),
            EnterpriseNumberType.HungaryAdoszam => HungaryAdoszamValidator.ValidateAdoszam(number),
            EnterpriseNumberType.IcelandKennitala => IcelandKennitalaValidator.ValidateKennitala(number),
            EnterpriseNumberType.IrelandVat => IrelandVatNumberValidator.ValidateVatNumber(number),
            EnterpriseNumberType.ItalyCodiceFiscale => ItalyPartitaIvaValidator.ValidatePartitaIvaStatic(number),
            EnterpriseNumberType.KosovoFiscalNumber => KosovoFiscalNumberValidator.ValidateFiscalNumber(number),
            EnterpriseNumberType.LatviaPvn => LatviaPvnValidator.ValidatePvn(number),
            EnterpriseNumberType.LiechtensteinPeid => LiechtensteinPeidValidator.ValidatePeid(number),
            EnterpriseNumberType.LithuaniaPvm => LithuaniaPvmValidator.ValidatePvm(number),
            EnterpriseNumberType.LuxembourgTva => LuxembourgVatValidator.ValidateVat(number),
            EnterpriseNumberType.MaltaVat => MaltaVatValidator.ValidateVat(number),
            EnterpriseNumberType.MoldovaIdno => MoldovaIdnoValidator.ValidateIdno(number),
            EnterpriseNumberType.MonacoRci => MonacoRciValidator.ValidateRci(number),
            EnterpriseNumberType.MontenegroPib => MontenegroPibValidator.ValidatePib(number),
            EnterpriseNumberType.NetherlandsBtw => NetherlandsVatValidator.ValidateBtw(number),
            EnterpriseNumberType.NorthMacedoniaEdb => NorthMacedoniaEdbValidator.ValidateEdb(number),
            EnterpriseNumberType.NorwayOrgNumber => NorwayOrgNumberValidator.ValidateOrgNumber(number),
            EnterpriseNumberType.PolandNip => PolandNipValidator.ValidateNip(number),
            EnterpriseNumberType.PortugalNif => PortugalNifValidator.ValidateNif(number),
            EnterpriseNumberType.RomaniaCif => RomaniaCifValidator.ValidateCif(number),
            EnterpriseNumberType.SanMarinoCoe => SanMarinoCoeValidator.ValidateCoe(number),
            EnterpriseNumberType.SerbiaPib => SerbiaPibValidator.ValidatePib(number),
            EnterpriseNumberType.SlovakiaVat => SlovakiaVatValidator.ValidateVat(number),
            EnterpriseNumberType.SloveniaTaxNumber => SloveniaVatValidator.ValidateVat(number),
            EnterpriseNumberType.SpainCif => SpainCifValidator.ValidateCif(number),
            EnterpriseNumberType.SwedenMoms => SwedenVatValidator.ValidateVat(number),
            EnterpriseNumberType.SwitzerlandUid => SwitzerlandUidValidator.ValidateUid(number),
            EnterpriseNumberType.TurkeyVkn => TurkeyVknValidator.ValidateVkn(number),
            EnterpriseNumberType.UkraineEdrpou => UkraineEdrpouValidator.ValidateEdrpou(number),
            EnterpriseNumberType.UnitedKingdomCompanyNumber => UnitedKingdomCompanyNumberValidator.ValidateCompanyNumber(number),
            EnterpriseNumberType.VaticanCityVat => VaticanCityVatValidator.ValidateVat(number),
            _ => ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, $"Enterprise number type {type} is not supported.")
        };
    }

    public static ValidationResult ValidateEnterpriseNumber(string? number, string countryCode)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        if (string.IsNullOrWhiteSpace(countryCode))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        string country = countryCode.ToUpperInvariant();

        switch (country)
        {
            case "AL":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.AlbaniaNipt);
            case "AD":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.AndorraNrt);
            case "AT":
                if (number.StartsWith("AT", StringComparison.OrdinalIgnoreCase))
                {
                    return ValidateEnterpriseNumber(number[2..], EnterpriseNumberType.AustriaFirmenbuch);
                }
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.AustriaFirmenbuch);
            case "AZ":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.AzerbaijanVoen);
            case "BY":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.BelarusUnp);
            case "BE":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.BelgiumEnterpriseNumber);
            case "BA":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.BosniaJib);
            case "BG":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.BulgariaUic);
            case "HR":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.CroatiaOib);
            case "CY":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.CyprusTic);
            case "CZ":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.CzechRepublicIco);
            case "DK":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.DenmarkCvr);
            case "EE":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.EstoniaRegistrikood);
            case "FR":
                string frNumber = number;
                if (number.StartsWith("FR", StringComparison.OrdinalIgnoreCase))
                {
                    frNumber = number[2..];
                }

                if (frNumber.Length == 9)
                {
                    return ValidateEnterpriseNumber(frNumber, EnterpriseNumberType.FranceSiren);
                }
                return ValidateEnterpriseNumber(frNumber, EnterpriseNumberType.FranceSiret);
            case "FO":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.FaroeIslandsVtal);
            case "FI":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.FinlandBusinessId);
            case "GE":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.GeorgiaTaxId);
            case "DE":
                // Try Handelsregisternummer first (HRA/HRB prefix)
                if (GermanyHandelsregisternummerValidator.ValidateHandelsregisternummer(number).IsValid)
                {
                    return ValidateEnterpriseNumber(number, EnterpriseNumberType.GermanyHandelsregisternummer);
                }
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.GermanySteuernummer);
            case "GI":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.GibraltarCompanyNumber);
            case "GR":
            case "EL":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.GreeceAfm);
            case "GL":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.GreenlandCvr);
            case "HU":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.HungaryAdoszam);
            case "IS":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.IcelandKennitala);
            case "IE":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.IrelandVat);
            case "IT":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.ItalyCodiceFiscale);
            case "XK":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.KosovoFiscalNumber);
            case "LV":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.LatviaPvn);
            case "LI":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.LiechtensteinPeid);
            case "LT":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.LithuaniaPvm);
            case "LU":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.LuxembourgTva);
            case "MT":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.MaltaVat);
            case "MD":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.MoldovaIdno);
            case "MC":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.MonacoRci);
            case "ME":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.MontenegroPib);
            case "NL":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.NetherlandsBtw);
            case "MK":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.NorthMacedoniaEdb);
            case "NO":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.NorwayOrgNumber);
            case "PL":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.PolandNip);
            case "PT":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.PortugalNif);
            case "RO":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.RomaniaCif);
            case "SM":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.SanMarinoCoe);
            case "RS":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.SerbiaPib);
            case "SK":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.SlovakiaVat);
            case "SI":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.SloveniaTaxNumber);
            case "ES":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.SpainCif);
            case "SE":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.SwedenMoms);
            case "CH":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.SwitzerlandUid);
            case "TR":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.TurkeyVkn);
            case "UA":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.UkraineEdrpou);
            case "GB":
            case "UK":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.UnitedKingdomCompanyNumber);
            case "VA":
                return ValidateEnterpriseNumber(number, EnterpriseNumberType.VaticanCityVat);
            default:
                return ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, string.Format(ValidationMessages.UnsupportedCountryCode, country));
        }
    }

    public static string? GetNormalizedNumber(string? number, string countryCode)
    {
        return countryCode switch
        {
            "AL" => new AlbaniaNiptValidator().Parse(number),
            "AD" => new AndorraNrtValidator().Parse(number),
            "AT" => new AustriaFirmenbuchValidator().Parse(number),
            "AZ" => new AzerbaijanVoenValidator().Parse(number),
            "BY" => new BelarusUnpValidator().Parse(number),
            "BE" => new BelgiumEnterpriseValidator().Parse(number),
            "BA" => new BosniaJibValidator().Parse(number),
            "BG" => new BulgariaUicValidator().Parse(number),
            "HR" => new CroatiaOibValidator().Parse(number),
            "CY" => new CyprusTicValidator().Parse(number),
            "CZ" => new CzechRepublicIcoValidator().Parse(number),
            "DK" => new DenmarkCvrValidator().Parse(number),
            "EE" => new EstoniaRegistrikoodValidator().Parse(number),
            "FR" => ParseFranceNumber(number),
            "FO" => new FaroeIslandsVtalValidator().Parse(number),
            "FI" => new FinlandBusinessIdValidator().Parse(number),
            "GE" => new GeorgiaTaxIdValidator().Parse(number),
            "DE" => GermanyHandelsregisternummerValidator.ValidateHandelsregisternummer(number).IsValid
                    ? GermanyHandelsregisternummerValidator.Normalize(number)
                    : GermanySteuernummerValidator.Normalize(number),
            "GI" => new GibraltarCompanyNumberValidator().Parse(number),
            "GR" or "EL" => new GreeceAfmValidator().Parse(number),
            "GL" => new GreenlandCvrValidator().Parse(number),
            "HU" => new HungaryAdoszamValidator().Parse(number),
            "IS" => new IcelandKennitalaValidator().Parse(number),
            "IE" => new IrelandVatNumberValidator().Parse(number),
            "IT" => ItalyPartitaIvaValidator.Normalize(number),
            "XK" => new KosovoFiscalNumberValidator().Parse(number),
            "LV" => new LatviaPvnValidator().Parse(number),
            "LI" => new LiechtensteinPeidValidator().Parse(number),
            "LT" => new LithuaniaPvmValidator().Parse(number),
            "LU" => LuxembourgVatValidator.Normalize(number),
            "MT" => MaltaVatValidator.Normalize(number),
            "MD" => new MoldovaIdnoValidator().Parse(number),
            "MC" => new MonacoRciValidator().Parse(number),
            "ME" => new MontenegroPibValidator().Parse(number),
            "NL" => NetherlandsVatValidator.Normalize(number),
            "MK" => new NorthMacedoniaEdbValidator().Normalize(number),
            "NO" => NorwayOrgNumberValidator.Normalize(number),
            "PL" => new PolandNipValidator().Normalize(number),
            "PT" => new PortugalNifValidator().Parse(number),
            "RO" => new RomaniaCifValidator().Normalize(number),
            "SM" => new SanMarinoCoeValidator().Normalize(number),
            "RS" => new SerbiaPibValidator().Normalize(number),
            "SK" => new SlovakiaVatValidator().Normalize(number),
            "SI" => SloveniaVatValidator.Normalize(number),
            "ES" => SpainCifValidator.Normalize(number),
            "SE" => SwedenVatValidator.Normalize(number),
            "CH" => new SwitzerlandUidValidator().Parse(number),
            "TR" => TurkeyVknValidator.Normalize(number),
            "UA" => UkraineEdrpouValidator.Normalize(number),
            "GB" or "UK" => new UnitedKingdomCompanyNumberValidator().Parse(number),
            "VA" => VaticanCityVatValidator.Normalize(number),
            _ => null
        };
    }

    public static string? GetNormalizedNumber(string? number, EnterpriseNumberType type)
    {
        return type switch
        {
            EnterpriseNumberType.AlbaniaNipt => new AlbaniaNiptValidator().Parse(number),
            EnterpriseNumberType.AndorraNrt => new AndorraNrtValidator().Parse(number),
            EnterpriseNumberType.AustriaFirmenbuch => new AustriaFirmenbuchValidator().Parse(number),
            EnterpriseNumberType.AzerbaijanVoen => new AzerbaijanVoenValidator().Parse(number),
            EnterpriseNumberType.BelarusUnp => new BelarusUnpValidator().Parse(number),
            EnterpriseNumberType.BelgiumEnterpriseNumber => new BelgiumEnterpriseValidator().Parse(number),
            EnterpriseNumberType.BosniaJib => new BosniaJibValidator().Parse(number),
            EnterpriseNumberType.BulgariaUic => new BulgariaUicValidator().Parse(number),
            EnterpriseNumberType.CroatiaOib => new CroatiaOibValidator().Parse(number),
            EnterpriseNumberType.CyprusTic => new CyprusTicValidator().Parse(number),
            EnterpriseNumberType.CzechRepublicIco => new CzechRepublicIcoValidator().Parse(number),
            EnterpriseNumberType.DenmarkCvr => new DenmarkCvrValidator().Parse(number),
            EnterpriseNumberType.EstoniaRegistrikood => new EstoniaRegistrikoodValidator().Parse(number),
            EnterpriseNumberType.FaroeIslandsVtal => new FaroeIslandsVtalValidator().Parse(number),
            EnterpriseNumberType.FinlandBusinessId => new FinlandBusinessIdValidator().Parse(number),
            EnterpriseNumberType.FranceSiren => new FranceSirenValidator().Parse(number),
            EnterpriseNumberType.FranceSiret => new FranceSiretValidator().Parse(number),
            EnterpriseNumberType.GeorgiaTaxId => new GeorgiaTaxIdValidator().Parse(number),
            EnterpriseNumberType.GermanySteuernummer => GermanySteuernummerValidator.Normalize(number),
            EnterpriseNumberType.GermanyHandelsregisternummer => GermanyHandelsregisternummerValidator.Normalize(number),
            EnterpriseNumberType.GibraltarCompanyNumber => new GibraltarCompanyNumberValidator().Parse(number),
            EnterpriseNumberType.GreeceAfm => new GreeceAfmValidator().Parse(number),
            EnterpriseNumberType.GreenlandCvr => new GreenlandCvrValidator().Parse(number),
            EnterpriseNumberType.HungaryAdoszam => new HungaryAdoszamValidator().Parse(number),
            EnterpriseNumberType.IcelandKennitala => new IcelandKennitalaValidator().Parse(number),
            EnterpriseNumberType.IrelandVat => new IrelandVatNumberValidator().Parse(number),
            EnterpriseNumberType.ItalyCodiceFiscale => ItalyPartitaIvaValidator.Normalize(number),
            EnterpriseNumberType.KosovoFiscalNumber => new KosovoFiscalNumberValidator().Parse(number),
            EnterpriseNumberType.LatviaPvn => new LatviaPvnValidator().Parse(number),
            EnterpriseNumberType.LiechtensteinPeid => new LiechtensteinPeidValidator().Parse(number),
            EnterpriseNumberType.LithuaniaPvm => new LithuaniaPvmValidator().Parse(number),
            EnterpriseNumberType.LuxembourgTva => LuxembourgVatValidator.Normalize(number),
            EnterpriseNumberType.MaltaVat => MaltaVatValidator.Normalize(number),
            EnterpriseNumberType.MoldovaIdno => new MoldovaIdnoValidator().Parse(number),
            EnterpriseNumberType.MonacoRci => new MonacoRciValidator().Parse(number),
            EnterpriseNumberType.MontenegroPib => new MontenegroPibValidator().Parse(number),
            EnterpriseNumberType.NetherlandsBtw => NetherlandsVatValidator.Normalize(number),
            EnterpriseNumberType.NorthMacedoniaEdb => new NorthMacedoniaEdbValidator().Normalize(number),
            EnterpriseNumberType.NorwayOrgNumber => NorwayOrgNumberValidator.Normalize(number),
            EnterpriseNumberType.PolandNip => new PolandNipValidator().Normalize(number),
            EnterpriseNumberType.PortugalNif => new PortugalNifValidator().Parse(number),
            EnterpriseNumberType.RomaniaCif => new RomaniaCifValidator().Normalize(number),
            EnterpriseNumberType.SanMarinoCoe => new SanMarinoCoeValidator().Normalize(number),
            EnterpriseNumberType.SerbiaPib => new SerbiaPibValidator().Normalize(number),
            EnterpriseNumberType.SlovakiaVat => new SlovakiaVatValidator().Normalize(number),
            EnterpriseNumberType.SloveniaTaxNumber => SloveniaVatValidator.Normalize(number),
            EnterpriseNumberType.SpainCif => SpainCifValidator.Normalize(number),
            EnterpriseNumberType.SwedenMoms => SwedenVatValidator.Normalize(number),
            EnterpriseNumberType.SwitzerlandUid => new SwitzerlandUidValidator().Parse(number),
            EnterpriseNumberType.TurkeyVkn => TurkeyVknValidator.Normalize(number),
            EnterpriseNumberType.UkraineEdrpou => UkraineEdrpouValidator.Normalize(number),
            EnterpriseNumberType.UnitedKingdomCompanyNumber => new UnitedKingdomCompanyNumberValidator().Parse(number),
            EnterpriseNumberType.VaticanCityVat => VaticanCityVatValidator.Normalize(number),
            _ => null
        };
    }

    private static string? ParseFranceNumber(string? number)
    {
        if (number == null) return null;

        string cleanNumber = number;
        if (number.StartsWith("FR", StringComparison.OrdinalIgnoreCase) && number.Length > 2)
        {
            cleanNumber = number[2..];
        }

        return cleanNumber.Length == 9
            ? new FranceSirenValidator().Parse(cleanNumber)
            : new FranceSiretValidator().Parse(cleanNumber);
    }
}
