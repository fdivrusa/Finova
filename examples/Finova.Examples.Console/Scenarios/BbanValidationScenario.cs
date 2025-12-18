using Finova.Core.Identifiers;
using Finova.Countries.Europe.Austria.Validators;
using Finova.Countries.Europe.Belgium.Validators;
using Finova.Countries.Europe.Bulgaria.Validators;
using Finova.Countries.Europe.Croatia.Validators;
using Finova.Countries.Europe.Cyprus.Validators;
using Finova.Countries.Europe.CzechRepublic.Validators;
using Finova.Countries.Europe.Denmark.Validators;
using Finova.Countries.Europe.Estonia.Validators;
using Finova.Countries.Europe.Finland.Validators;
using Finova.Countries.Europe.France.Validators;
using Finova.Countries.Europe.Germany.Validators;
using Finova.Countries.Europe.Greece.Validators;
using Finova.Countries.Europe.Hungary.Validators;
using Finova.Countries.Europe.Ireland.Validators;
using Finova.Countries.Europe.Italy.Validators;
using Finova.Countries.Europe.Latvia.Validators;
using Finova.Countries.Europe.Lithuania.Validators;
using Finova.Countries.Europe.Luxembourg.Validators;
using Finova.Countries.Europe.Malta.Validators;
using Finova.Countries.Europe.Netherlands.Validators;
using Finova.Countries.Europe.Poland.Validators;
using Finova.Countries.Europe.Portugal.Validators;
using Finova.Countries.Europe.Romania.Validators;
using Finova.Countries.Europe.Slovakia.Validators;
using Finova.Countries.Europe.Slovenia.Validators;
using Finova.Countries.Europe.Spain.Validators;
using Finova.Countries.Europe.Sweden.Validators;
using Finova.Countries.Europe.Switzerland.Validators;
using Finova.Countries.Europe.UnitedKingdom.Validators;
using Finova.Examples.ConsoleApp.Helpers;
using Finova.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Finova.Examples.ConsoleApp.Scenarios;

public static class BbanValidationScenario
{
    public static void Run()
    {
        ConsoleHelper.WriteSectionHeader("PART 7: BBAN VALIDATION (NEW)");

        RunStaticValidation();
        RunDependencyInjectionValidation();
    }

    private static void RunStaticValidation()
    {
        ConsoleHelper.WriteSubHeader("34", "Static BBAN Validation (Zero Allocation)");
        ConsoleHelper.WriteCode("BelgiumBbanValidator.Validate(bban)");

        // Austria (16 chars)
        var atBban = "1904300234573201"; 
        var atResult = AustriaBbanValidator.Validate(atBban);
        ConsoleHelper.WriteResult("Austria", atBban, atResult.IsValid, atResult.Errors.FirstOrDefault()?.Message);

        // Belgium (12 chars)
        var beBban = "539007547034"; 
        var beResult = BelgiumBbanValidator.Validate(beBban);
        ConsoleHelper.WriteResult("Belgium", beBban, beResult.IsValid, beResult.Errors.FirstOrDefault()?.Message);

        // Bulgaria (18 chars)
        var bgBban = "STSA93000123456789"; 
        var bgResult = BulgariaBbanValidator.Validate(bgBban);
        ConsoleHelper.WriteResult("Bulgaria", bgBban, bgResult.IsValid, bgResult.Errors.FirstOrDefault()?.Message);

        // Croatia (17 chars)
        var hrBban = "23600001101234565"; 
        var hrResult = CroatiaBbanValidator.Validate(hrBban);
        ConsoleHelper.WriteResult("Croatia", hrBban, hrResult.IsValid, hrResult.Errors.FirstOrDefault()?.Message);

        // Cyprus (24 chars)
        var cyBban = "002001950000357001234567"; 
        var cyResult = CyprusBbanValidator.Validate(cyBban);
        ConsoleHelper.WriteResult("Cyprus", cyBban, cyResult.IsValid, cyResult.Errors.FirstOrDefault()?.Message);

        // Czech Republic (20 chars)
        var czBban = "08000000192000145399"; 
        var czResult = CzechRepublicBbanValidator.Validate(czBban);
        ConsoleHelper.WriteResult("Czech Rep.", czBban, czResult.IsValid, czResult.Errors.FirstOrDefault()?.Message);

        // Denmark (14 chars)
        var dkBban = "00400440116243"; 
        var dkResult = DenmarkBbanValidator.Validate(dkBban);
        ConsoleHelper.WriteResult("Denmark", dkBban, dkResult.IsValid, dkResult.Errors.FirstOrDefault()?.Message);

        // Estonia (16 chars)
        var eeBban = "1000001020145686"; 
        var eeResult = EstoniaBbanValidator.Validate(eeBban);
        ConsoleHelper.WriteResult("Estonia", eeBban, eeResult.IsValid, eeResult.Errors.FirstOrDefault()?.Message);

        // Finland (14 chars)
        var fiBban = "12345600000785"; 
        var fiResult = FinlandBbanValidator.Validate(fiBban);
        ConsoleHelper.WriteResult("Finland", fiBban, fiResult.IsValid, fiResult.Errors.FirstOrDefault()?.Message);

        // France (23 chars)
        var frBban = "30006000011234567890189"; 
        var frResult = FranceBbanValidator.Validate(frBban);
        ConsoleHelper.WriteResult("France", frBban, frResult.IsValid, frResult.Errors.FirstOrDefault()?.Message);

        // Germany (18 chars)
        var deBban = "370400440532013000"; 
        var deResult = GermanyBbanValidator.Validate(deBban);
        ConsoleHelper.WriteResult("Germany", deBban, deResult.IsValid, deResult.Errors.FirstOrDefault()?.Message);

        // Greece (23 chars)
        var grBban = "01101250000000012300695"; 
        var grResult = GreeceBbanValidator.Validate(grBban);
        ConsoleHelper.WriteResult("Greece", grBban, grResult.IsValid, grResult.Errors.FirstOrDefault()?.Message);

        // Hungary (24 chars)
        var huBban = "117730161111101800000000"; 
        var huResult = HungaryBbanValidator.Validate(huBban);
        ConsoleHelper.WriteResult("Hungary", huBban, huResult.IsValid, huResult.Errors.FirstOrDefault()?.Message);

        // Ireland (22 chars)
        var ieBban = "AIBK93115212345678"; 
        var ieResult = IrelandBbanValidator.Validate(ieBban);
        ConsoleHelper.WriteResult("Ireland", ieBban, ieResult.IsValid, ieResult.Errors.FirstOrDefault()?.Message);

        // Italy (23 chars)
        var itBban = "X0542811101000000123456"; 
        var itResult = ItalyBbanValidator.Validate(itBban);
        ConsoleHelper.WriteResult("Italy", itBban, itResult.IsValid, itResult.Errors.FirstOrDefault()?.Message);

        // Latvia (17 chars)
        var lvBban = "HABA0012345678910"; 
        var lvResult = LatviaBbanValidator.Validate(lvBban);
        ConsoleHelper.WriteResult("Latvia", lvBban, lvResult.IsValid, lvResult.Errors.FirstOrDefault()?.Message);

        // Lithuania (16 chars)
        var ltBban = "1010012345678901"; 
        var ltResult = LithuaniaBbanValidator.Validate(ltBban);
        ConsoleHelper.WriteResult("Lithuania", ltBban, ltResult.IsValid, ltResult.Errors.FirstOrDefault()?.Message);

        // Luxembourg (16 chars)
        var luBban = "0019400644750000"; 
        var luResult = LuxembourgBbanValidator.Validate(luBban);
        ConsoleHelper.WriteResult("Luxembourg", luBban, luResult.IsValid, luResult.Errors.FirstOrDefault()?.Message);

        // Malta (27 chars)
        var mtBban = "MALT01100000000000000000123"; 
        var mtResult = MaltaBbanValidator.Validate(mtBban);
        ConsoleHelper.WriteResult("Malta", mtBban, mtResult.IsValid, mtResult.Errors.FirstOrDefault()?.Message);

        // Netherlands (14 chars)
        var nlBban = "ABNA0417164300"; 
        var nlResult = NetherlandsBbanValidator.Validate(nlBban);
        ConsoleHelper.WriteResult("Netherlands", nlBban, nlResult.IsValid, nlResult.Errors.FirstOrDefault()?.Message);

        // Poland (24 chars)
        var plBban = "109010140000071219812874"; 
        var plResult = PolandBbanValidator.Validate(plBban);
        ConsoleHelper.WriteResult("Poland", plBban, plResult.IsValid, plResult.Errors.FirstOrDefault()?.Message);

        // Portugal (21 chars)
        var ptBban = "500000201231234567863"; 
        var ptResult = PortugalBbanValidator.Validate(ptBban);
        ConsoleHelper.WriteResult("Portugal", ptBban, ptResult.IsValid, ptResult.Errors.FirstOrDefault()?.Message);

        // Romania (20 chars)
        var roBban = "AAAA1B31007593840000"; 
        var roResult = RomaniaBbanValidator.Validate(roBban);
        ConsoleHelper.WriteResult("Romania", roBban, roResult.IsValid, roResult.Errors.FirstOrDefault()?.Message);

        // Slovakia (20 chars)
        var skBban = "75000000000012345671"; 
        var skResult = SlovakiaBbanValidator.Validate(skBban);
        ConsoleHelper.WriteResult("Slovakia", skBban, skResult.IsValid, skResult.Errors.FirstOrDefault()?.Message);

        // Slovenia (15 chars)
        var siBban = "192001234567892"; 
        var siResult = SloveniaBbanValidator.Validate(siBban);
        ConsoleHelper.WriteResult("Slovenia", siBban, siResult.IsValid, siResult.Errors.FirstOrDefault()?.Message);

        // Spain (20 chars)
        var esBban = "21000418450200051332"; 
        var esResult = SpainBbanValidator.Validate(esBban);
        ConsoleHelper.WriteResult("Spain", esBban, esResult.IsValid, esResult.Errors.FirstOrDefault()?.Message);

        // Sweden (20 chars)
        var seBban = "50000000058398257466"; 
        var seResult = SwedenBbanValidator.Validate(seBban);
        ConsoleHelper.WriteResult("Sweden", seBban, seResult.IsValid, seResult.Errors.FirstOrDefault()?.Message);

        // Switzerland (17 chars)
        var chBban = "04835012345678009"; 
        var chResult = SwitzerlandBbanValidator.Validate(chBban);
        ConsoleHelper.WriteResult("Switzerland", chBban, chResult.IsValid, chResult.Errors.FirstOrDefault()?.Message);

        // UK (18 chars)
        var ukBban = "NWBK60161331926819"; 
        var ukResult = UnitedKingdomBbanValidator.Validate(ukBban);
        ConsoleHelper.WriteResult("UK", ukBban, ukResult.IsValid, ukResult.Errors.FirstOrDefault()?.Message);
    }

    private static void RunDependencyInjectionValidation()
    {
        ConsoleHelper.WriteSubHeader("35", "IBbanValidator via DI");
        
        var services = new ServiceCollection();
        services.AddFinova();
        var provider = services.BuildServiceProvider();

        // Resolve all validators
        var validators = provider.GetServices<IBbanValidator>().OrderBy(v => v.CountryCode).ToList();
        Console.WriteLine($"Registered BBAN Validators: {validators.Count()}");

        // Test data map
        var testData = new Dictionary<string, string>
        {
            { "AT", "1904300234573201" },
            { "BE", "539007547034" },
            { "BG", "STSA93000123456789" },
            { "CH", "04835012345678009" },
            { "CY", "002001950000357001234567" },
            { "CZ", "08000000192000145399" },
            { "DE", "370400440532013000" },
            { "DK", "00400440116243" },
            { "EE", "1000001020145686" },
            { "ES", "21000418450200051332" },
            { "FI", "12345600000785" },
            { "FR", "30006000011234567890189" },
            { "GB", "NWBK60161331926819" },
            { "GR", "01101250000000012300695" },
            { "HR", "23600001101234565" },
            { "HU", "117730161111101800000000" },
            { "IE", "AIBK93115212345678" },
            { "IT", "X0542811101000000123456" },
            { "LT", "1010012345678901" },
            { "LU", "0019400644750000" },
            { "LV", "HABA0012345678910" },
            { "MT", "MALT01100000000000000000123" },
            { "NL", "ABNA0417164300" },
            { "PL", "109010140000071219812874" },
            { "PT", "500000201231234567863" },
            { "RO", "AAAA1B31007593840000" },
            { "SE", "50000000058398257466" },
            { "SI", "192001234567892" },
            { "SK", "75000000000012345671" }
        };

        foreach (var validator in validators)
        {
            if (testData.TryGetValue(validator.CountryCode, out var bban))
            {
                var result = validator.Validate(bban);
                ConsoleHelper.WriteResult($"DI - {validator.CountryCode}", bban, result.IsValid, result.Errors.FirstOrDefault()?.Message);
            }
            else
            {
                Console.WriteLine($"No test data for {validator.CountryCode}");
            }
        }
    }
}
