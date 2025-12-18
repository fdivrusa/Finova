using Finova.Belgium.Services;
using Finova.Belgium.Validators;
using Finova.Core.Bic;
using Finova.Core.Common;
using Finova.Core.PaymentCard;
using Finova.Core.PaymentReference;
using Finova.Core.PaymentReference.Internals;
using Finova.Core.Vat;
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
using Finova.Countries.Europe.Denmark.Services;
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
using Finova.Countries.Europe.Italy.Services;
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
using Finova.Countries.Europe.Portugal.Services;
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
using Finova.Examples.ConsoleApp.Helpers;
using Finova.Services;
using Finova.Validators;

namespace Finova.Examples.ConsoleApp.Scenarios;

public static class StaticValidationScenario
{
    public static void Run()
    {
        ConsoleHelper.WriteSectionHeader("PART 1: STATIC USAGE (Direct, High-Performance)");

        RunBicValidation();
        RunMultiCountryIbanValidation();
        RunEuropeIbanValidation();
        RunPaymentCardValidation();
        RunPaymentReferenceValidation();
        RunVatValidation();
        RunNationalIdValidation();
    }

    private static void RunBicValidation()
    {
        ConsoleHelper.WriteSubHeader("1", "BIC/SWIFT Code Validation (ISO 9362)");
        ConsoleHelper.WriteCode("BicValidator.Validate(bic).IsValid");

        string[] bicCodes =
        [
            "GEBABEBB",       // BNP Paribas Fortis (Belgium)
            "KREDBEBB",       // KBC Bank (Belgium)
            "BNPAFRPP",       // BNP Paribas (France)
            "COBADEFF",       // Commerzbank (Germany)
            "ABNANL2A",       // ABN AMRO (Netherlands)
            "BCITITMM",       // Intesa Sanpaolo (Italy)
            "BABORULU",       // Banque Raiffeisen (Luxembourg) - Invalid country code!
            "BGLLLULL",       // Banque de Luxembourg
            "CABORUBEXXX",    // Invalid - RU country code mismatch
            "NWBKGB2L",       // NatWest (United Kingdom)
            "CAIXESBB",       // CaixaBank (Spain)
            "GEBABEBB021",    // With branch code (11 chars)
            "INVALID",        // Invalid
            "ABC"             // Too short
        ];

        foreach (var bic in bicCodes)
        {
            bool isValid = BicValidator.Validate(bic).IsValid;
            ConsoleHelper.WriteSimpleResult(bic, isValid);
        }

        Console.WriteLine();
    }

    private static void RunMultiCountryIbanValidation()
    {
        ConsoleHelper.WriteSubHeader("2", "Multi-Country IBAN Validation");
        ConsoleHelper.WriteCode("CountrySpecificValidator.Validate(iban)");

        var ibansToCheck = new (string Country, string Iban, Func<string, ValidationResult> Validator)[]
        {
            ("Albania 🇦🇱", "AL96100100010000000000000001", AlbaniaIbanValidator.ValidateAlbaniaIban),
            ("Andorra 🇦🇩", "AD1400080001001234567890", AndorraIbanValidator.ValidateAndorraIban),
            ("Austria 🇦🇹", "AT611904300234573201", AustriaIbanValidator.ValidateAustriaIban),
            ("Azerbaijan 🇦🇿", "AZ21NABZ00000000137010001944", AzerbaijanIbanValidator.ValidateAzerbaijanIban),
            ("Belarus 🇧🇾", "BY29BAPB30000000000000000001", BelarusIbanValidator.ValidateBelarusIban),
            ("Belgium 🇧🇪", "BE68539007547034", BelgiumIbanValidator.ValidateBelgiumIban),
            ("Bosnia 🇧🇦", "BA783930000000000000", BosniaAndHerzegovinaIbanValidator.ValidateBosniaAndHerzegovinaIban),
            ("Bulgaria 🇧🇬", "BG19STSA93000123456789", BulgariaIbanValidator.ValidateBulgariaIban),
            ("Croatia 🇭🇷", "HR1723600001101234565", CroatiaIbanValidator.ValidateCroatiaIban),
            ("Cyprus 🇨🇾", "CY21002001950000357001234567", CyprusIbanValidator.ValidateCyprusIban),
            ("Czech Republic 🇨🇿", "CZ6508000000192000145399", CzechRepublicIbanValidator.ValidateCzechIban),
            ("Denmark 🇩🇰", "DK5000400440116243", DenmarkIbanValidator.ValidateDenmarkIban),
            ("Estonia 🇪🇪", "EE201000001020145686", EstoniaIbanValidator.ValidateEstoniaIban),
            ("Faroe Islands 🇫🇴", "FO7460000000000011", FaroeIslandsIbanValidator.ValidateFaroeIslandsIban),
            ("Finland 🇫🇮", "FI2112345600000785", FinlandIbanValidator.ValidateFinlandIban),
            ("France 🇫🇷", "FR7630006000011234567890189", FranceIbanValidator.ValidateFranceIban),
            ("Georgia 🇬🇪", "GE6329NB00000001019049", GeorgiaIbanValidator.ValidateGeorgiaIban),
            ("Germany 🇩🇪", "DE89370400440532013000", GermanyIbanValidator.ValidateGermanyIban),
            ("Gibraltar 🇬🇮", "GI56XAPO000001234567890", GibraltarIbanValidator.ValidateGibraltarIban),
            ("Greece 🇬🇷", "GR1601101250000000012300695", GreeceIbanValidator.ValidateGreeceIban),
            ("Greenland 🇬🇱", "GL5360000000000001", GreenlandIbanValidator.ValidateGreenlandIban),
            ("Hungary 🇭🇺", "HU42117730161111101800000000", HungaryIbanValidator.ValidateHungaryIban),
            ("Iceland 🇮🇸", "IS750001121234563108962099", IcelandIbanValidator.ValidateIcelandIban),
            ("Ireland 🇮🇪", "IE29AIBK93115212345678", IrelandIbanValidator.ValidateIrelandIban),
            ("Italy 🇮🇹", "IT60X0542811101000000123456", ItalyIbanValidator.ValidateItalyIban),
            ("Kosovo 🇽🇰", "XK950505000000000000", KosovoIbanValidator.ValidateKosovoIban),
            ("Latvia 🇱🇻", "LV97HABA0012345678910", LatviaIbanValidator.ValidateLatviaIban),
            ("Liechtenstein 🇱🇮", "LI2200000000000000001", LiechtensteinIbanValidator.ValidateLiechtensteinIban),
            ("Lithuania 🇱🇹", "LT601010012345678901", LithuaniaIbanValidator.ValidateLithuaniaIban),
            ("Luxembourg 🇱🇺", "LU280019400644750000", LuxembourgIbanValidator.ValidateLuxembourgIban),
            ("Malta 🇲🇹", "MT31MALT01100000000000000000123", MaltaIbanValidator.ValidateMaltaIban),
            ("Moldova 🇲🇩", "MD76AA100000000000000001", MoldovaIbanValidator.ValidateMoldovaIban),
            ("Monaco 🇲🇨", "MC5810096180790123456789085", MonacoIbanValidator.ValidateMonacoIban),
            ("Montenegro 🇲🇪", "ME36500000000000000001", MontenegroIbanValidator.ValidateMontenegroIban),
            ("Netherlands 🇳🇱", "NL91ABNA0417164300", NetherlandsIbanValidator.ValidateNetherlandsIban),
            ("North Macedonia 🇲🇰", "MK31100000000000001", NorthMacedoniaIbanValidator.ValidateNorthMacedoniaIban),
            ("Norway 🇳🇴", "NO9386011117947", NorwayIbanValidator.ValidateNorwayIban),
            ("Poland 🇵🇱", "PL61109010140000071219812874", PolandIbanValidator.ValidatePolandIban),
            ("Portugal 🇵🇹", "PT03500000201231234567863", PortugalIbanValidator.ValidatePortugalIban),
            ("Romania 🇷🇴", "RO49AAAA1B31007593840000", RomaniaIbanValidator.ValidateRomaniaIban),
            ("San Marino 🇸🇲", "SM76P0854009812123456789123", SanMarinoIbanValidator.ValidateSanMarinoIban),
            ("Serbia 🇷🇸", "RS35260005601001611379", SerbiaIbanValidator.ValidateSerbiaIban),
            ("Slovakia 🇸🇰", "SK8975000000000012345671", SlovakiaIbanValidator.ValidateSlovakiaIban),
            ("Slovenia 🇸🇮", "SI56192001234567892", SloveniaIbanValidator.ValidateSloveniaIban),
            ("Spain 🇪🇸", "ES9121000418450200051332", SpainIbanValidator.ValidateSpainIban),
            ("Sweden 🇸🇪", "SE4550000000058398257466", SwedenIbanValidator.ValidateSwedenIban),
            ("Switzerland 🇨🇭", "CH5604835012345678009", SwitzerlandIbanValidator.ValidateSwitzerlandIban),
            ("Turkey 🇹🇷", "TR960006100000000000000001", TurkeyIbanValidator.ValidateTurkeyIban),
            ("Ukraine 🇺🇦", "UA443000230000000000000000000", UkraineIbanValidator.ValidateUkraineIban),
            ("United Kingdom 🇬🇧", "GB29NWBK60161331926819", UnitedKingdomIbanValidator.ValidateUnitedKingdomIban),
            ("Vatican 🇻🇦", "VA59001123000012345678", VaticanIbanValidator.ValidateVaticanIban)
        };

        foreach (var (country, iban, validator) in ibansToCheck)
        {
            ConsoleHelper.WriteCountryHeader(country.Substring(country.Length - 2), country);
            ConsoleHelper.WriteSimpleResult(iban, validator(iban).IsValid);
        }

        Console.WriteLine();
    }

    private static void RunEuropeIbanValidation()
    {
        ConsoleHelper.WriteSubHeader("3", "Europe-Wide IBAN Validation (Wrapper)");
        ConsoleHelper.WriteCode("EuropeIbanValidator.ValidateIban(iban).IsValid");

        string[] europeanIbans =
        [
            "AT611904300234573201",       // Austria
            "BE68539007547034",           // Belgium
            "CZ6508000000192000145399",   // Czech Republic
            "DK5000400440116243",         // Denmark
            "FI2112345600000785",         // Finland
            "FR7630006000011234567890189", // France
            "DE89370400440532013000",     // Germany
            "GR1601101250000000012300695", // Greece
            "HU42117730161111101800000000", // Hungary
            "IE29AIBK93115212345678",     // Ireland
            "IT60X0542811101000000123456", // Italy
            "LU280019400644750000",       // Luxembourg
            "NL91ABNA0417164300",         // Netherlands
            "NO9386011117947",            // Norway
            "PL61109010140000071219812874", // Poland
            "RO49AAAA1B31007593840000",   // Romania
            "RS35260005601001611379",     // Serbia
            "ES9121000418450200051332",   // Spain
            "SE4550000000058398257466",   // Sweden
            "GB29NWBK60161331926819",     // United Kingdom
            "XX00123456789012"            // Unknown country (will fail)
        ];

        foreach (var iban in europeanIbans)
        {
            bool isValid = EuropeIbanValidator.ValidateIban(iban).IsValid;
            string country = iban.Length >= 2 ? iban[..2] : "??";
            ConsoleHelper.WriteSimpleResult(iban, isValid, country);
        }

        Console.WriteLine();
    }

    private static void RunPaymentCardValidation()
    {
        ConsoleHelper.WriteSubHeader("4", "Payment Card Validation (Luhn Algorithm)");
        ConsoleHelper.WriteCode("PaymentCardValidator.Validate(number).IsValid");

        var testCards = new (string Number, string Description)[]
        {
            ("4111111111111111", "Visa"),
            ("4111 1111 1111 1111", "Visa (spaces)"),
            ("4532015112830366", "Visa"),
            ("5500000000000004", "Mastercard"),
            ("5425233430109903", "Mastercard"),
            ("340000000000009", "Amex"),
            ("378282246310005", "Amex"),
            ("6011000000000004", "Discover"),
            ("3530111333300000", "JCB"),
            ("36000000000008", "Diners Club"),
            ("1234567890123456", "Invalid"),
        };

        foreach (var (number, desc) in testCards)
        {
            bool isValid = PaymentCardValidator.Validate(number).IsValid;
            var brand = PaymentCardValidator.GetBrand(number);

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($"      {desc,-15} ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{number,-22} ");

            if (isValid)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("✓ Valid   ");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("✗ Invalid ");
            }

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"[{brand}]");
            Console.ResetColor();
        }

        Console.WriteLine();
    }

    private static void RunPaymentReferenceValidation()
    {
        // ─────────────────────────────────────────
        // 5. ISO 11649 Payment Reference (RF)
        // ─────────────────────────────────────────
        ConsoleHelper.WriteSubHeader("5", "ISO 11649 Payment Reference (RF)");
        ConsoleHelper.WriteCode("PaymentReferenceValidator.Validate(reference).IsValid");

        // Generate valid RF references for testing
        string validRf1 = IsoReferenceHelper.Generate("539007547034");
        string validRf2 = IsoReferenceHelper.Generate("INV2024001234");
        string validRf3 = IsoReferenceHelper.Generate("ABC123");

        string[] references = [validRf1, validRf2, validRf3, "RF00123456789", "INVALID"];
        foreach (var reference in references)
        {
            ConsoleHelper.WriteSimpleResult(reference, PaymentReferenceValidator.Validate(reference).IsValid);
        }

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("      Generate RF Reference from arbitrary input:");
        Console.ResetColor();

        try
        {
            string generated = IsoReferenceHelper.Generate("INVOICE2024ABC");
            ConsoleHelper.WriteInfo("Input", "INVOICE2024ABC");
            ConsoleHelper.WriteInfo("Generated", generated);
            ConsoleHelper.WriteSuccess($"Validation passed: {PaymentReferenceValidator.Validate(generated).IsValid}");
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError(ex.Message);
        }

        Console.WriteLine();

        // ─────────────────────────────────────────
        // 5b. PaymentReferenceValidator Facade (Static with Format)
        // ─────────────────────────────────────────
        ConsoleHelper.WriteSubHeader("5b", "PaymentReferenceValidator Facade (Static with Format)");
        ConsoleHelper.WriteCode("PaymentReferenceValidator.Validate(reference, format)");

        var facadeExamples = new (string Ref, PaymentReferenceFormat Format)[]
        {
            ("RF18539007547034", PaymentReferenceFormat.IsoRf),
            ("+++090/9337/55493+++", PaymentReferenceFormat.LocalBelgian),
            ("+71<123456789012347", PaymentReferenceFormat.LocalDenmark)
        };

        foreach (var (reference, format) in facadeExamples)
        {
            var result = PaymentReferenceValidator.Validate(reference, format);
            ConsoleHelper.WriteSimpleResult($"{format}", result.IsValid, reference);
        }

        Console.WriteLine();

        // ─────────────────────────────────────────
        // 6. Belgian Structured Payment Reference (OGM/VCS)
        // ─────────────────────────────────────────
        ConsoleHelper.WriteSubHeader("6", "Belgian Structured Payment Reference (OGM/VCS)");
        ConsoleHelper.WriteCode("BelgiumPaymentReferenceService.ValidateStatic(ogm1).IsValid");

        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("      Generate Belgian OGM from communication number:");
        Console.ResetColor();

        try
        {
            var belgiumRefService = new BelgiumPaymentReferenceService();

            // Generate from different inputs (max 10 digits for OGM)
            string ogm1 = belgiumRefService.Generate("1234567890");
            string ogm2 = belgiumRefService.Generate("1");
            string ogm3 = belgiumRefService.Generate("9999999999");

            ConsoleHelper.WriteInfo("Input 1234567890", $"→ {ogm1}");
            ConsoleHelper.WriteInfo("Input 0000000001", $"→ {ogm2}");
            ConsoleHelper.WriteInfo("Input 9999999999", $"→ {ogm3}");

            // Validate
            ConsoleHelper.WriteSimpleResult($"Validate: {ogm1}", BelgiumPaymentReferenceService.ValidateStatic(ogm1).IsValid);
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError(ex.Message);
        }

        Console.WriteLine();

        // ─────────────────────────────────────────
        // 7. Denmark Payment Reference (FIK/GIK)
        // ─────────────────────────────────────────
        ConsoleHelper.WriteSubHeader("7", "Denmark Payment Reference (FIK/GIK)");
        ConsoleHelper.WriteCode("DenmarkPaymentReferenceService.ValidateStatic(item.Ref)");

        try
        {
            var denmarkReferences = new[]
            {
                (Label: "FIK 71 (Valid)", Ref: "+71<123456789012347"),
                (Label: "FIK 04 (Valid)", Ref: "+04<123456789012347"),
                (Label: "FIK 71 (Invalid Checksum)", Ref: "+71<123456789012340"),
                (Label: "FIK (Too Short)", Ref: "+71<123"),
                (Label: "Full OCR (Unsupported)", Ref: "+71<12345678901234+12345678<")
            };

            foreach (var item in denmarkReferences)
            {
                var result = DenmarkPaymentReferenceService.ValidateStatic(item.Ref);
                string? errorMsg = result.IsValid ? null : string.Join(", ", result.Errors.Select(e => e.Message));
                ConsoleHelper.WriteResult(item.Label, item.Ref, result.IsValid, errorMsg);
            }
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError(ex.Message);
        }

        Console.WriteLine();

        // ─────────────────────────────────────────
        // 8. Italy Payment Reference (CBILL/PagoPA)
        // ─────────────────────────────────────────
        ConsoleHelper.WriteSubHeader("8", "Italy Payment Reference (CBILL/PagoPA)");
        ConsoleHelper.WriteCode("ItalyPaymentReferenceService.ValidateStatic(validCbill).IsValid");

        try
        {
            // Generate a valid CBILL/PagoPA reference (18 digits)
            // Input: 17 digits -> Output: 18 digits (with check digit)
            string validCbill = ItalyPaymentReferenceService.GenerateStatic("12345678901234567");
            ConsoleHelper.WriteResult("CBILL (Valid)", validCbill, ItalyPaymentReferenceService.ValidateStatic(validCbill).IsValid);

            // Invalid example
            string invalidCbill = "123456789012345678";
            ConsoleHelper.WriteResult("CBILL (Invalid)", invalidCbill, ItalyPaymentReferenceService.ValidateStatic(invalidCbill).IsValid, "Invalid Checksum");
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError(ex.Message);
        }

        Console.WriteLine();

        // ─────────────────────────────────────────
        // 9. Portugal Payment Reference (Multibanco)
        // ─────────────────────────────────────────
        ConsoleHelper.WriteSubHeader("9", "Portugal Payment Reference (Multibanco)");
        ConsoleHelper.WriteCode("PortugalPaymentReferenceService.ValidateStatic(multibanco).IsValid");

        try
        {
            // Multibanco (9 digits)
            string multibanco = "123456789";
            ConsoleHelper.WriteResult("Multibanco", multibanco, PortugalPaymentReferenceService.ValidateStatic(multibanco).IsValid);
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError(ex.Message);
        }

        Console.WriteLine();
    }

    private static void RunVatValidation()
    {
        ConsoleHelper.WriteSubHeader("10", "European VAT & Enterprise Number Validation");
        ConsoleHelper.WriteCode("EuropeVatValidator.ValidateVat(vat)");

        var vatExamples = new (string Country, string Vat)[]
        {
            ("Albania", "ALJ91402501L"),
            ("Andorra", "ADU123456A"), // Format check only
            ("Austria", "ATU13585627"),
            ("Azerbaijan", "AZ1234567890"), // Format check
            ("Belarus", "BY100000007"),
            ("Belgium", "BE0403019261"),
            ("Bosnia", "BA4000000000005"),
            ("Bulgaria", "BG175074752"),
            ("Croatia", "HR94577403194"),
            ("Cyprus", "CY10259033P"),
            ("Czech Republic", "CZ25123891"),
            ("Denmark", "DK13585628"),
            ("Estonia", "EE100931558"),
            ("Faroe Islands", "FO123456"), // Format check
            ("Finland", "FI01080233"),
            ("France", "FR40303265045"),
            ("Georgia", "GE123456789"), // Format check
            ("Germany", "DE122119035"),
            ("Greece", "EL094014201"),
            ("Hungary", "HU12892312"),
            ("Iceland", "IS123456"), // Format check
            ("Ireland", "IE6388047V"),
            ("Italy", "IT00000010215"),
            ("Kosovo", "XK100000009"),
            ("Latvia", "LV40003521600"),
            ("Liechtenstein", "LI107787577"),
            ("Lithuania", "LT100001911"),
            ("Luxembourg", "LU15027442"),
            ("Malta", "MT11679112"),
            ("Moldova", "MD1234567890123"), // Format check
            ("Monaco", "MC44732829320"),
            ("Montenegro", "ME10000004"),
            ("Netherlands", "NL004495445B01"),
            ("North Macedonia", "MK4030992255006"),
            ("Norway", "NO999999999MVA"), // Format check
            ("Poland", "PL5260300291"),
            ("Portugal", "PT502757191"),
            ("Romania", "RO18547290"),
            ("San Marino", "SM12345"), // Format check
            ("Serbia", "RS100000024"),
            ("Slovakia", "SK2020273893"),
            ("Slovenia", "SI50223054"),
            ("Spain", "ESA12345674"),
            ("Sweden", "SE556000461501"),
            ("Switzerland", "CHE107787577"),
            ("Turkey", "TR1234567890"), // Format check
            ("Ukraine", "UA12345678"), // Format check
            ("United Kingdom", "GB434031494"),
            ("Vatican", "VA00462350018")
        };

        foreach (var (country, vat) in vatExamples)
        {
            var result = EuropeVatValidator.ValidateVat(vat);
            ConsoleHelper.WriteSimpleResult($"{country} ({vat})", result.IsValid, result.IsValid ? "Valid" : result.Errors.FirstOrDefault()?.Message);
        }

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("      Formatting:");
        Console.ResetColor();

        try
        {
            ConsoleHelper.WriteInfo("VAT", $"0764117795 → {BelgiumVatValidator.Format("0764117795")}");
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError(ex.Message);
        }
    }

    private static void RunNationalIdValidation()
    {
        ConsoleHelper.WriteSubHeader("7", "National ID Validation (Static)");
        ConsoleHelper.WriteCode("EuropeNationalIdValidator.Validate(country, id).IsValid");

        var examples = new[]
        {
            ("Belgium", "BE", "72020290081", true, "Valid NN"),
            ("Belgium", "BE", "72020290082", false, "Invalid Checksum"),
            ("France", "FR", "1 80 01 45 000 000 69", true, "Valid NIR"),
            ("France", "FR", "1 80 01 45 000 000 00", false, "Invalid Checksum"),
            ("Germany", "DE", "T22000124", true, "Valid Steuer-ID"),
            ("Italy", "IT", "RSSMRA80A01H501U", true, "Valid Codice Fiscale"),
            ("Spain", "ES", "12345678Z", true, "Valid DNI"),
            ("Sweden", "SE", "8112189876", true, "Valid Personnummer"),
            ("UK", "GB", "QQ123456A", true, "Valid NINO"),
            ("Norway", "NO", "01010012356", true, "Valid Fodselsnummer"),
            ("Finland", "FI", "131052-308T", true, "Valid Henkilotunnus")
        };

        foreach (var (country, code, id, expected, desc) in examples)
        {
            var result = EuropeNationalIdValidator.Validate(code, id);
            ConsoleHelper.WriteSimpleResult($"{country} ({id})", result.IsValid, result.IsValid ? "Valid" : result.Errors.FirstOrDefault()?.Message);
        }

        Console.WriteLine();
    }
}
