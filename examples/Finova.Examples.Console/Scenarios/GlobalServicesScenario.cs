using Finova.Core.Identifiers;
using Finova.Examples.ConsoleApp.Helpers;
using Finova.Extensions;
using Finova.Services;
using Finova.Core.Iban;
using Microsoft.Extensions.DependencyInjection;

namespace Finova.Examples.ConsoleApp.Scenarios;

public static class GlobalServicesScenario
{
    public static void Run()
    {
        ConsoleHelper.WriteSectionHeader("PART 5: GLOBAL SERVICES (Tax ID & Bank Account Services)");

        // Setup DI Container
        var services = new ServiceCollection();
        services.AddFinova();
        var serviceProvider = services.BuildServiceProvider();

        RunGlobalTaxIdService(serviceProvider);
        RunGlobalBankAccountService(serviceProvider);
        RunEuropeIbanValidator(serviceProvider);
        RunGlobalIbanValidator();
        RunGlobalAdaptersForEurope(serviceProvider);
        RunRegionalBankValidators(serviceProvider);
        RunGlobalBankValidatorFacade();
    }

    private static void RunGlobalTaxIdService(ServiceProvider provider)
    {
        ConsoleHelper.WriteSubHeader("24", "Global Tax ID Service (ITaxIdService)");
        ConsoleHelper.WriteCode("taxIdService.Validate(country, id)");
        var taxIdService = provider.GetRequiredService<ITaxIdService>();

        string[] taxIds = [
            "US", "12-3456789", // US EIN
            "BR", "12.345.678/0001-95", // Brazil CNPJ
            "CN", "91350100M000100Y43", // China USCC
            "IN", "ABCPE1234F", // India PAN
            "SG", "200812345M" // Singapore UEN
        ];

        for (int i = 0; i < taxIds.Length; i += 2)
        {
            string country = taxIds[i];
            string id = taxIds[i+1];
            var result = taxIdService.Validate(country, id);
            ConsoleHelper.WriteSimpleResult($"{country}: {id}", result.IsValid, result.IsValid ? "Valid" : (result.Errors.FirstOrDefault()?.Message ?? "Invalid"));
        }
    }

    private static void RunGlobalBankAccountService(ServiceProvider provider)
    {
        ConsoleHelper.WriteSubHeader("25", "Global Bank Account Service (IBankAccountService)");
        ConsoleHelper.WriteCode("bankAccountService.Validate(country, acc)");
        var bankAccountService = provider.GetRequiredService<IBankAccountService>();

        string[] bankAccounts = [
            "SG", "1234567890", // Singapore (Valid)
            "JP", "1234567", // Japan (Valid)
            "JP", "123456", // Japan (Invalid length)
            "SG", "123" // Singapore (Invalid length)
        ];

        for (int i = 0; i < bankAccounts.Length; i += 2)
        {
            string country = bankAccounts[i];
            string acc = bankAccounts[i+1];
            var result = bankAccountService.Validate(country, acc);
            ConsoleHelper.WriteSimpleResult($"{country}: {acc}", result.IsValid, result.IsValid ? "Valid" : (result.Errors.FirstOrDefault()?.Message ?? "Invalid"));
        }
    }

    private static void RunEuropeIbanValidator(ServiceProvider provider)
    {
        ConsoleHelper.WriteSubHeader("26", "Europe IBAN Validator (via DI)");
        ConsoleHelper.WriteCode("europeValidator.Validate(iban)");
        // EuropeIbanValidator is registered as IIbanValidator
        var europeValidator = provider.GetServices<IIbanValidator>().OfType<EuropeIbanValidator>().FirstOrDefault();

        if (europeValidator != null)
        {
            var europeResult = europeValidator.Validate("BE68539007547034");
            ConsoleHelper.WriteSimpleResult("BE68539007547034", europeResult.IsValid, "Validated via DI (EuropeIbanValidator)");
        }
        else
        {
            Console.WriteLine("EuropeIbanValidator not found in DI.");
        }
    }

    private static void RunGlobalIbanValidator()
    {
        ConsoleHelper.WriteSubHeader("26b", "Global IBAN Validator (Static - All Regions)");
        ConsoleHelper.WriteCode("GlobalIbanValidator.ValidateIban(iban)");

        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("      Validating IBANs from all supported regions:");
        Console.ResetColor();
        Console.WriteLine();

        // Middle East IBANs
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("      === MIDDLE EAST ===");
        Console.ResetColor();

        var middleEastIbans = new[]
        {
            new { Iban = "AE070331234567890123456", Country = "United Arab Emirates", Valid = true },
            new { Iban = "BH67BMAG00001299123456", Country = "Bahrain", Valid = true },
            new { Iban = "IL620108000000099999999", Country = "Israel", Valid = true },
            new { Iban = "JO94CBJO0010000000000131000302", Country = "Jordan", Valid = true },
            new { Iban = "KW81CBKU0000000000001234560101", Country = "Kuwait", Valid = true },
            new { Iban = "LB62099900000001001901229114", Country = "Lebanon", Valid = true },
            new { Iban = "QA58DOHB00001234567890ABCDEFG", Country = "Qatar", Valid = true },
            new { Iban = "SA0380000000608010167519", Country = "Saudi Arabia", Valid = true }
        };

        foreach (var test in middleEastIbans)
        {
            var result = GlobalIbanValidator.ValidateIban(test.Iban);
            ConsoleHelper.WriteSimpleResult($"{test.Country}: {test.Iban}", result.IsValid,
                result.IsValid ? "Valid" : (result.Errors.FirstOrDefault()?.Message ?? "Invalid"));
        }
        Console.WriteLine();

        // Africa IBANs
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("      === AFRICA ===");
        Console.ResetColor();

        var africaIbans = new[]
        {
            new { Iban = "EG380019000500000000263180002", Country = "Egypt", Valid = true },
            new { Iban = "MR1300020001010000123456753", Country = "Mauritania", Valid = true },
            new { Iban = "TN5910006035183598478831", Country = "Tunisia (Europe validator)", Valid = true }
        };

        foreach (var test in africaIbans)
        {
            var result = GlobalIbanValidator.ValidateIban(test.Iban);
            ConsoleHelper.WriteSimpleResult($"{test.Country}: {test.Iban}", result.IsValid,
                result.IsValid ? "Valid" : (result.Errors.FirstOrDefault()?.Message ?? "Invalid"));
        }
        Console.WriteLine();

        // Americas IBANs
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("      === AMERICAS ===");
        Console.ResetColor();

        var americasIbans = new[]
        {
            new { Iban = "BR1500000000000010932840814P2", Country = "Brazil", Valid = true },
            new { Iban = "CR05015202001026284066", Country = "Costa Rica", Valid = true },
            new { Iban = "DO28BAGR00000001212453611324", Country = "Dominican Republic", Valid = true },
            new { Iban = "SV62CENR00000000000000700025", Country = "El Salvador", Valid = true },
            new { Iban = "GT82TRAJ01020000001210029690", Country = "Guatemala", Valid = true },
            new { Iban = "VG96VPVG0000012345678901", Country = "Virgin Islands (British)", Valid = true }
        };

        foreach (var test in americasIbans)
        {
            var result = GlobalIbanValidator.ValidateIban(test.Iban);
            ConsoleHelper.WriteSimpleResult($"{test.Country}: {test.Iban}", result.IsValid,
                result.IsValid ? "Valid" : (result.Errors.FirstOrDefault()?.Message ?? "Invalid"));
        }
        Console.WriteLine();

        // Asia & Pacific IBANs
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("      === ASIA & PACIFIC ===");
        Console.ResetColor();

        var asiaIbans = new[]
        {
            new { Iban = "KZ86125KZT5004100100", Country = "Kazakhstan", Valid = true },
            new { Iban = "PK36SCBL0000001123456702", Country = "Pakistan", Valid = true },
            new { Iban = "TL380080012345678910157", Country = "Timor-Leste", Valid = true }
        };

        foreach (var test in asiaIbans)
        {
            var result = GlobalIbanValidator.ValidateIban(test.Iban);
            ConsoleHelper.WriteSimpleResult($"{test.Country}: {test.Iban}", result.IsValid,
                result.IsValid ? "Valid" : (result.Errors.FirstOrDefault()?.Message ?? "Invalid"));
        }
        Console.WriteLine();

        // European IBANs (via GlobalIbanValidator routing)
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("      === EUROPE (via GlobalIbanValidator) ===");
        Console.ResetColor();

        var europeIbans = new[]
        {
            new { Iban = "DE89370400440532013000", Country = "Germany", Valid = true },
            new { Iban = "FR7630006000011234567890189", Country = "France", Valid = true },
            new { Iban = "GB29NWBK60161331926819", Country = "United Kingdom", Valid = true },
            new { Iban = "NL91ABNA0417164300", Country = "Netherlands", Valid = true }
        };

        foreach (var test in europeIbans)
        {
            var result = GlobalIbanValidator.ValidateIban(test.Iban);
            ConsoleHelper.WriteSimpleResult($"{test.Country}: {test.Iban}", result.IsValid,
                result.IsValid ? "Valid" : (result.Errors.FirstOrDefault()?.Message ?? "Invalid"));
        }
        Console.WriteLine();

        // Invalid IBANs
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("      === INVALID IBANS (Various Errors) ===");
        Console.ResetColor();

        var invalidIbans = new[]
        {
            new { Iban = "XX123456789012345", Country = "Unknown Country Code", Error = "Unsupported country" },
            new { Iban = "DE89370400440532013001", Country = "Germany (Wrong Checksum)", Error = "Checksum error" },
            new { Iban = "BH67BMAG000012991234", Country = "Bahrain (Too Short)", Error = "Length error" },
            new { Iban = "", Country = "Empty Input", Error = "Empty input" }
        };

        foreach (var test in invalidIbans)
        {
            var result = GlobalIbanValidator.ValidateIban(test.Iban);
            ConsoleHelper.WriteSimpleResult($"{test.Country}: {(string.IsNullOrEmpty(test.Iban) ? "(empty)" : test.Iban)}", result.IsValid,
                result.IsValid ? "Valid" : (result.Errors.FirstOrDefault()?.Message ?? test.Error));
        }
        Console.WriteLine();
    }

    private static void RunGlobalAdaptersForEurope(ServiceProvider provider)
    {
        ConsoleHelper.WriteSubHeader("27", "Global Adapters for Europe");

        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("      Demonstrating usage of Global Services (ITaxIdService) for European identifiers:");
        Console.ResetColor();

        var taxIdService = provider.GetRequiredService<ITaxIdService>();
        var bankAccountService = provider.GetRequiredService<IBankAccountService>();

        // 1. Validate French VAT using ITaxIdService
        string frVat = "FR44732829320"; // Valid VAT (BNP Paribas)
        ConsoleHelper.WriteCode("taxIdService.Validate(\"FR\", frVat)");
        var frVatResult = taxIdService.Validate("FR", frVat);
        ConsoleHelper.WriteInfo("Validate French VAT via ITaxIdService", $"Input: {frVat}");
        if (frVatResult.IsValid)
        {
            ConsoleHelper.WriteSuccess("Valid! (Delegated to FranceVatValidator)");
        }
        else
        {
            ConsoleHelper.WriteError("Invalid!");
        }

        // 2. Validate French SIREN using ITaxIdService
        string frSiren = "732 829 320"; // Valid SIREN (BNP Paribas)
        ConsoleHelper.WriteCode("taxIdService.Validate(\"FR\", frSiren)");
        var frSirenResult = taxIdService.Validate("FR", frSiren);
        ConsoleHelper.WriteInfo("Validate French SIREN via ITaxIdService", $"Input: {frSiren}");
        if (frSirenResult.IsValid)
        {
            ConsoleHelper.WriteSuccess("Valid! (Delegated to FranceSirenValidator)");
        }
        else
        {
            ConsoleHelper.WriteError("Invalid!");
        }

        // 3. Validate Belgian IBAN using IBankAccountService
        string beIban = "BE68539007547034";
        ConsoleHelper.WriteCode("bankAccountService.Validate(\"BE\", beIban)");
        var beIbanResult = bankAccountService.Validate("BE", beIban);
        ConsoleHelper.WriteInfo("Validate Belgian IBAN via IBankAccountService", $"Input: {beIban}");
        if (beIbanResult.IsValid)
        {
            ConsoleHelper.WriteSuccess("Valid! (Delegated to BelgiumIbanValidator)");
        }
        else
        {
            ConsoleHelper.WriteError("Invalid!");
        }

        Console.WriteLine();
    }

    private static void RunGlobalBankValidatorFacade()
    {
        ConsoleHelper.WriteSubHeader("29", "Global Bank Validator Facade (Static)");

        Console.WriteLine("      Demonstrating usage of GlobalBankValidator (Static Facade):");
        Console.WriteLine();

        // Routing Numbers
        var routingTests = new[]
        {
            new { Country = "US", Routing = "121000248", Desc = "Valid US ABA" },
            new { Country = "CA", Routing = "000112345", Desc = "Valid CA Routing" },
            new { Country = "US", Routing = "123456789", Desc = "Invalid US Checksum" }
        };

        ConsoleHelper.WriteCode("GlobalBankValidator.ValidateRoutingNumber(country, routing)");
        foreach (var test in routingTests)
        {
            var result = GlobalBankValidator.ValidateRoutingNumber(test.Country, test.Routing);
            ConsoleHelper.WriteSimpleResult($"{test.Country} Routing: {test.Routing}", result.IsValid, test.Desc);
        }
        Console.WriteLine();

        // Account Numbers
        var accountTests = new[]
        {
            new { Country = "SG", Account = "1234567890", Desc = "Valid SG Account" },
            new { Country = "JP", Account = "1234567", Desc = "Valid JP Account" },
            new { Country = "SG", Account = "123", Desc = "Invalid SG Length" }
        };

        ConsoleHelper.WriteCode("GlobalBankValidator.ValidateBankAccount(country, account)");
        foreach (var test in accountTests)
        {
            var result = GlobalBankValidator.ValidateBankAccount(test.Country, test.Account);
            ConsoleHelper.WriteSimpleResult($"{test.Country} Account: {test.Account}", result.IsValid, test.Desc);
        }
        Console.WriteLine();
    }

    private static void RunRegionalBankValidators(ServiceProvider provider)
    {
        ConsoleHelper.WriteSubHeader("28", "Regional Bank Validators (DI)");

        // Europe
        var europeValidator = provider.GetRequiredService<EuropeBankValidator>();
        ConsoleHelper.WriteCode("europeValidator.ValidateRouting(\"DE\", \"10070024\")");
        var deResult = europeValidator.ValidateRouting("DE", "10070024");
        ConsoleHelper.WriteSimpleResult("Europe (DE BLZ)", deResult.IsValid, deResult.IsValid ? "Valid" : "Invalid");

        // North America
        var naValidator = provider.GetRequiredService<NorthAmericaBankValidator>();
        ConsoleHelper.WriteCode("naValidator.ValidateRouting(\"US\", \"121000248\")");
        var usResult = naValidator.ValidateRouting("US", "121000248");
        ConsoleHelper.WriteSimpleResult("North America (US ABA)", usResult.IsValid, usResult.IsValid ? "Valid" : "Invalid");

        // Asia
        var asiaValidator = provider.GetRequiredService<AsiaBankValidator>();
        ConsoleHelper.WriteCode("asiaValidator.ValidateRouting(\"CN\", \"102100099996\")");
        var cnResult = asiaValidator.ValidateRouting("CN", "102100099996");
        ConsoleHelper.WriteSimpleResult("Asia (CN CNAPS)", cnResult.IsValid, cnResult.IsValid ? "Valid" : "Invalid");
    }
}
