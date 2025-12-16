using Finova.Countries.Asia.China.Validators;
using Finova.Countries.Asia.India.Validators;
using Finova.Countries.Asia.Japan.Validators;
using Finova.Countries.Asia.Singapore.Validators;
using Finova.Countries.NorthAmerica.Canada.Validators;
using Finova.Countries.NorthAmerica.UnitedStates.Validators;
using Finova.Countries.Oceania.Australia.Validators;
using Finova.Countries.SouthAmerica.Brazil.Validators;
using Finova.Countries.SouthAmerica.Mexico.Validators;
using Finova.Examples.ConsoleApp.Helpers;

namespace Finova.Examples.ConsoleApp.Scenarios;

public static class GlobalExpansionScenario
{
    public static void Run()
    {
        ConsoleHelper.WriteSectionHeader("PART 4: GLOBAL EXPANSION (v1.4.0)");
        RunNorthAmerica();
        RunSouthAmerica();
        RunAsia();
        RunOceania();
    }

    private static void RunNorthAmerica()
    {
        ConsoleHelper.WriteSubHeader("17", "North America (USA & Canada)");

        ConsoleHelper.WriteCountryHeader("🇺🇸", "United States");
        ConsoleHelper.WriteResult("Routing Number", "011000015", new UnitedStatesRoutingNumberValidator().Validate("011000015").IsValid);
        ConsoleHelper.WriteResult("EIN", "12-3456789", new UnitedStatesEinValidator().Validate("12-3456789").IsValid);

        ConsoleHelper.WriteCountryHeader("🇨🇦", "Canada");
        ConsoleHelper.WriteResult("SIN", "046 454 286", new CanadaSinValidator().Validate("046 454 286").IsValid);
        ConsoleHelper.WriteResult("BN", "100000009", new CanadaBusinessNumberValidator().Validate("100000009").IsValid);
    }

    private static void RunSouthAmerica()
    {
        ConsoleHelper.WriteSubHeader("18", "South America (Brazil & Mexico)");

        ConsoleHelper.WriteCountryHeader("🇧🇷", "Brazil");
        ConsoleHelper.WriteResult("CPF", "529.982.247-25", new BrazilCpfValidator().Validate("529.982.247-25").IsValid);
        ConsoleHelper.WriteResult("CNPJ", "11.444.777/0001-61", new BrazilCnpjValidator().Validate("11.444.777/0001-61").IsValid);

        ConsoleHelper.WriteCountryHeader("🇲🇽", "Mexico");
        ConsoleHelper.WriteResult("CURP", "HEGT760825HDFRXX03", MexicoCurpValidator.Validate("HEGT760825HDFRXX03").IsValid);
        ConsoleHelper.WriteResult("RFC", "XAXX010101000", MexicoRfcValidator.Validate("XAXX010101000").IsValid);
    }

    private static void RunAsia()
    {
        ConsoleHelper.WriteSubHeader("19", "Asia (China, Japan, India, Singapore)");

        ConsoleHelper.WriteCountryHeader("🇨🇳", "China");
        ConsoleHelper.WriteResult("RIC", "110101199003074514", new ChinaResidentIdentityCardValidator().Validate("110101199003074514").IsValid);
        ConsoleHelper.WriteResult("USCC", "911100006000373350", new ChinaUnifiedSocialCreditCodeValidator().Validate("911100006000373350").IsValid);

        ConsoleHelper.WriteCountryHeader("🇯🇵", "Japan");
        ConsoleHelper.WriteResult("My Number", "123456789012", JapanMyNumberValidator.Validate("123456789012").IsValid, "Invalid (Expected)");
        ConsoleHelper.WriteResult("Corporate Number", "3835600000001", JapanCorporateNumberValidator.Validate("3835600000001").IsValid);

        ConsoleHelper.WriteCountryHeader("🇮🇳", "India");
        ConsoleHelper.WriteResult("Aadhaar", "999999990019", new IndiaAadhaarValidator().Validate("999999990019").IsValid);
        ConsoleHelper.WriteResult("PAN", "ABCPE1234F", new IndiaPanValidator().Validate("ABCPE1234F").IsValid);

        ConsoleHelper.WriteCountryHeader("🇸🇬", "Singapore");
        ConsoleHelper.WriteResult("NRIC", "S1234567D", SingaporeNricValidator.Validate("S1234567D").IsValid);
        ConsoleHelper.WriteResult("UEN", "200812345M", SingaporeUenValidator.Validate("200812345M").IsValid);
    }

    private static void RunOceania()
    {
        ConsoleHelper.WriteSubHeader("20", "Oceania (Australia)");

        ConsoleHelper.WriteCountryHeader("🇦🇺", "Australia");
        ConsoleHelper.WriteResult("TFN", "123456782", new AustraliaTfnValidator().Validate("123456782").IsValid);
        ConsoleHelper.WriteResult("ABN", "51824753556", new AustraliaAbnValidator().Validate("51824753556").IsValid);

        Console.WriteLine();
    }
}
