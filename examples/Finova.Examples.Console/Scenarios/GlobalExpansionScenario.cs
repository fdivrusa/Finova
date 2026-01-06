using Finova.Countries.Asia.China.Validators;
using Finova.Countries.Asia.India.Validators;
using Finova.Countries.Asia.Japan.Validators;
using Finova.Countries.Asia.Singapore.Validators;
using Finova.Countries.NorthAmerica.Canada.Validators;
using Finova.Countries.NorthAmerica.UnitedStates.Validators;
using Finova.Countries.Oceania.Australia.Validators;
using Finova.Countries.SouthAmerica.Brazil.Validators;
using Finova.Countries.SouthAmerica.Mexico.Validators;
// New additions
using Finova.Countries.Europe.Russia.Validators;
using Finova.Countries.Africa.Morocco.Validators;
using Finova.Countries.Asia.Mongolia.Validators;
using Finova.Countries.MiddleEast.Oman.Validators;
using Finova.Countries.NorthAmerica.Barbados.Validators;
using Finova.Countries.SouthAmerica.FalklandIslands.Validators;
using Finova.Countries.Africa.Algeria.Validators;
using Finova.Countries.Africa.Tunisia.Validators;
using Finova.Countries.Africa.Egypt.Validators;
using Finova.Countries.Africa.Nigeria.Validators;
using Finova.Countries.Asia.Kazakhstan.Validators;
using Finova.Countries.SoutheastAsia.Vietnam.Validators;

using Finova.Examples.ConsoleApp.Helpers;
using Finova.Extensions;
using Finova.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Finova.Examples.ConsoleApp.Scenarios;

public static class GlobalExpansionScenario
{
    public static void Run()
    {
        ConsoleHelper.WriteSectionHeader("PART 4: GLOBAL EXPANSION (v1.4.0)");

        // Setup DI for demonstration
        var services = new ServiceCollection();
        services.AddFinova();
        var provider = services.BuildServiceProvider();

        RunNorthAmerica(provider);
        RunSouthAmerica(provider);
        RunAsia(provider);
        RunOceania(provider);
        RunNewAdditions(provider);
    }

    private static void RunNewAdditions(ServiceProvider provider)
    {
        ConsoleHelper.WriteSubHeader("24", "New Additions (v1.4.0+)");

        ConsoleHelper.WriteCountryHeader("🇷🇺", "Russia");
        ConsoleHelper.WriteResult("INN (Tax ID)", "7707083893", new RussiaInnValidator().Validate("7707083893").IsValid);
        ConsoleHelper.WriteResult("IBAN", "RU1204452560040702810400000000012", new RussiaIbanValidator().Validate("RU1204452560040702810400000000012").IsValid);

        ConsoleHelper.WriteCountryHeader("🇲🇦", "Morocco");
        ConsoleHelper.WriteResult("ICE (Business)", "001525487000088", new MoroccoIceValidator().Validate("001525487000088").IsValid);
        ConsoleHelper.WriteResult("IBAN", "MA64007810000252600000032955", new MoroccoIbanValidator().Validate("MA64007810000252600000032955").IsValid);

        ConsoleHelper.WriteCountryHeader("🇩🇿", "Algeria");
        ConsoleHelper.WriteResult("NIF", "000016001275946", new AlgeriaNifValidator().Validate("000016001275946").IsValid);
        ConsoleHelper.WriteResult("IBAN", "DZ8900100000000000000000", new AlgeriaIbanValidator().Validate("DZ8900100000000000000000").IsValid);

        ConsoleHelper.WriteCountryHeader("🇹🇳", "Tunisia");
        ConsoleHelper.WriteResult("Matricule Fiscal", "1234567A/B/M/000", new TunisiaMatriculeFiscalValidator().Validate("1234567A/B/M/000").IsValid);
        ConsoleHelper.WriteResult("IBAN", "TN5910006035183598478831", new TunisiaIbanValidator().Validate("TN5910006035183598478831").IsValid);

        ConsoleHelper.WriteCountryHeader("🇪🇬", "Egypt");
        ConsoleHelper.WriteResult("TRN", "100-200-300", new EgyptTaxRegistrationNumberValidator().Validate("100-200-300").IsValid);
        ConsoleHelper.WriteResult("IBAN", "EG380019000500000000263180002", new EgyptIbanValidator().Validate("EG380019000500000000263180002").IsValid);

        ConsoleHelper.WriteCountryHeader("🇰🇿", "Kazakhstan");
        ConsoleHelper.WriteResult("BIN/IIN", "980540003232", new KazakhstanBinValidator().Validate("980540003232").IsValid);
        ConsoleHelper.WriteResult("IBAN", "KZ86125KZT5004100100", new KazakhstanIbanValidator().Validate("KZ86125KZT5004100100").IsValid);

        ConsoleHelper.WriteCountryHeader("🇻🇳", "Vietnam");
        ConsoleHelper.WriteResult("MST (Tax ID)", "0100109106", new VietnamTaxIdValidator().Validate("0100109106").IsValid);

        ConsoleHelper.WriteCountryHeader("🇳🇬", "Nigeria");
        ConsoleHelper.WriteResult("TIN", "12345678", new NigeriaTinValidator().Validate("12345678").IsValid);

        ConsoleHelper.WriteCountryHeader("🇲🇳", "Mongolia");
        ConsoleHelper.WriteResult("IBAN", "MN330001000000000000", new MongoliaIbanValidator().Validate("MN330001000000000000").IsValid);

        ConsoleHelper.WriteCountryHeader("🇴🇲", "Oman");
        ConsoleHelper.WriteResult("VAT", "OM123456789012", new OmanVatValidator().Validate("OM123456789012").IsValid);
        ConsoleHelper.WriteResult("IBAN", "OM121231234567890123", new OmanIbanValidator().Validate("OM121231234567890123").IsValid);

        ConsoleHelper.WriteCountryHeader("🇧🇧", "Barbados");
        ConsoleHelper.WriteResult("IBAN", "BB00CITB00000000000012345678", new BarbadosIbanValidator().Validate("BB00CITB00000000000012345678").IsValid);

        ConsoleHelper.WriteCountryHeader("🇫🇰", "Falkland Islands");
        ConsoleHelper.WriteResult("IBAN", "FK00SC123456789012", new FalklandIslandsIbanValidator().Validate("FK00SC123456789012").IsValid);
    }

    private static void RunNorthAmerica(ServiceProvider provider)
    {
        ConsoleHelper.WriteSubHeader("20", "North America (USA & Canada)");

        ConsoleHelper.WriteCountryHeader("🇺🇸", "United States");
        
        // Direct Usage
        ConsoleHelper.WriteCode("new UnitedStatesRoutingNumberValidator().Validate(val)");
        ConsoleHelper.WriteResult("Routing Number (Direct)", "011000015", new UnitedStatesRoutingNumberValidator().Validate("011000015").IsValid);
        
        // DI Usage
        var usRoutingValidator = provider.GetRequiredService<UnitedStatesRoutingNumberValidator>();
        ConsoleHelper.WriteCode("provider.GetRequiredService<UnitedStatesRoutingNumberValidator>().Validate(val)");
        ConsoleHelper.WriteResult("Routing Number (DI)", "011000015", usRoutingValidator.Validate("011000015").IsValid);

        ConsoleHelper.WriteResult("EIN", "12-3456789", new UnitedStatesEinValidator().Validate("12-3456789").IsValid);

        ConsoleHelper.WriteCountryHeader("🇨🇦", "Canada");
        
        // Direct
        ConsoleHelper.WriteCode("new CanadaSinValidator().Validate(val)");
        ConsoleHelper.WriteResult("SIN", "046 454 286", new CanadaSinValidator().Validate("046 454 286").IsValid);
        
        // DI
        var caSinValidator = provider.GetRequiredService<CanadaSinValidator>();
        ConsoleHelper.WriteCode("provider.GetRequiredService<CanadaSinValidator>().Validate(val)");
        ConsoleHelper.WriteResult("SIN (DI)", "046 454 286", caSinValidator.Validate("046 454 286").IsValid);

        ConsoleHelper.WriteResult("BN", "100000009", new CanadaBusinessNumberValidator().Validate("100000009").IsValid);
        
        // Canada Routing (Missing before)
        ConsoleHelper.WriteResult("Routing Number", "000112345", new CanadaRoutingNumberValidator().Validate("000112345").IsValid);
    }

    private static void RunSouthAmerica(ServiceProvider provider)
    {
        ConsoleHelper.WriteSubHeader("21", "South America (Brazil & Mexico)");

        ConsoleHelper.WriteCountryHeader("🇧🇷", "Brazil");
        
        // Direct
        ConsoleHelper.WriteCode("new BrazilCpfValidator().Validate(val)");
        ConsoleHelper.WriteResult("CPF", "529.982.247-25", new BrazilCpfValidator().Validate("529.982.247-25").IsValid);
        
        // DI
        var cpfValidator = provider.GetRequiredService<BrazilCpfValidator>();
        ConsoleHelper.WriteCode("provider.GetRequiredService<BrazilCpfValidator>().Validate(val)");
        ConsoleHelper.WriteResult("CPF (DI)", "529.982.247-25", cpfValidator.Validate("529.982.247-25").IsValid);

        ConsoleHelper.WriteResult("CNPJ", "11.444.777/0001-61", new BrazilCnpjValidator().Validate("11.444.777/0001-61").IsValid);
        
        // Brazil Bank Code (Missing before)
        ConsoleHelper.WriteResult("Bank Code", "001", new BrazilBankCodeValidator().Validate("001").IsValid);

        ConsoleHelper.WriteCountryHeader("🇲🇽", "Mexico");
        ConsoleHelper.WriteCode("new MexicoCurpValidator().Validate(val)");
        ConsoleHelper.WriteResult("CURP", "HEGT760825HDFRXX03", new MexicoCurpValidator().Validate("HEGT760825HDFRXX03").IsValid);
        ConsoleHelper.WriteResult("RFC", "XAXX010101000", new MexicoRfcValidator().Validate("XAXX010101000").IsValid);
    }

    private static void RunAsia(ServiceProvider provider)
    {
        ConsoleHelper.WriteSubHeader("22", "Asia (China, Japan, India, Singapore)");

        ConsoleHelper.WriteCountryHeader("🇨🇳", "China");

        // Direct
        ConsoleHelper.WriteCode("new ChinaResidentIdentityCardValidator().Validate(val)");
        ConsoleHelper.WriteResult("RIC", "110101199003074514", new ChinaResidentIdentityCardValidator().Validate("110101199003074514").IsValid);
        
        // DI
        var ricValidator = provider.GetRequiredService<ChinaResidentIdentityCardValidator>();
        ConsoleHelper.WriteCode("provider.GetRequiredService<ChinaResidentIdentityCardValidator>().Validate(val)");
        ConsoleHelper.WriteResult("RIC (DI)", "110101199003074514", ricValidator.Validate("110101199003074514").IsValid);

        ConsoleHelper.WriteCode("ChinaUnifiedSocialCreditCodeValidator.ValidateUscc(val) [Static Only]");
        ConsoleHelper.WriteResult("USCC", "911100006000373350", ChinaUnifiedSocialCreditCodeValidator.ValidateUscc("911100006000373350").IsValid);
        
        // China CNAPS (Missing before)
        ConsoleHelper.WriteResult("CNAPS", "102100099996", new ChinaCnapsValidator().Validate("102100099996").IsValid);

        ConsoleHelper.WriteCountryHeader("🇯🇵", "Japan");
        ConsoleHelper.WriteCode("new JapanMyNumberValidator().Validate(val)");
        ConsoleHelper.WriteResult("My Number", "123456789012", new JapanMyNumberValidator().Validate("123456789012").IsValid, "Invalid (Expected)");
        ConsoleHelper.WriteResult("Corporate Number", "3835600000001", new JapanCorporateNumberValidator().Validate("3835600000001").IsValid);
        
        // Japan Bank Account (DI Only available)
        var jpBankValidator = provider.GetRequiredService<JapanBankAccountValidator>();
        ConsoleHelper.WriteCode("provider.GetRequiredService<JapanBankAccountValidator>().Validate(val)");
        ConsoleHelper.WriteResult("Bank Account (DI)", "1234567", jpBankValidator.Validate("1234567").IsValid);

        ConsoleHelper.WriteCountryHeader("🇮🇳", "India");
        ConsoleHelper.WriteCode("new IndiaAadhaarValidator().Validate(val)");
        ConsoleHelper.WriteResult("Aadhaar", "999999990019", new IndiaAadhaarValidator().Validate("999999990019").IsValid);
        ConsoleHelper.WriteResult("PAN", "ABCPE1234F", new IndiaPanValidator().Validate("ABCPE1234F").IsValid);
        
        // India IFSC (Missing before)
        ConsoleHelper.WriteResult("IFSC", "SBIN0001234", new IndiaIfscValidator().Validate("SBIN0001234").IsValid);

        ConsoleHelper.WriteCountryHeader("🇸🇬", "Singapore");
        ConsoleHelper.WriteCode("new SingaporeNricValidator().Validate(val)");
        ConsoleHelper.WriteResult("NRIC", "S1234567D", new SingaporeNricValidator().Validate("S1234567D").IsValid);
        ConsoleHelper.WriteResult("UEN", "200812345M", new SingaporeUenValidator().Validate("200812345M").IsValid);
        
        // Singapore Bank Account (DI Only available)
        var sgBankValidator = provider.GetRequiredService<SingaporeBankAccountValidator>();
        ConsoleHelper.WriteCode("provider.GetRequiredService<SingaporeBankAccountValidator>().Validate(val)");
        ConsoleHelper.WriteResult("Bank Account (DI)", "1234567890", sgBankValidator.Validate("1234567890").IsValid);
    }

    private static void RunOceania(ServiceProvider provider)
    {
        ConsoleHelper.WriteSubHeader("23", "Oceania (Australia)");

        ConsoleHelper.WriteCountryHeader("🇦🇺", "Australia");
        
        // Direct
        ConsoleHelper.WriteCode("new AustraliaTfnValidator().Validate(val)");
        ConsoleHelper.WriteResult("TFN", "123456782", new AustraliaTfnValidator().Validate("123456782").IsValid);
        
        // DI
        var tfnValidator = provider.GetRequiredService<AustraliaTfnValidator>();
        ConsoleHelper.WriteCode("provider.GetRequiredService<AustraliaTfnValidator>().Validate(val)");
        ConsoleHelper.WriteResult("TFN (DI)", "123456782", tfnValidator.Validate("123456782").IsValid);

        ConsoleHelper.WriteResult("ABN", "51824753556", new AustraliaAbnValidator().Validate("51824753556").IsValid);
        
        // Australia BSB (Missing before)
        ConsoleHelper.WriteResult("BSB", "033-088", new AustraliaBsbValidator().Validate("033-088").IsValid);

        Console.WriteLine();
    }

}
