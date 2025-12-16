using Finova.Core.Identifiers;
using Finova.Examples.ConsoleApp.Helpers;
using Finova.Extensions;
using Finova.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Finova.Examples.ConsoleApp.Scenarios;

public static class GlobalServicesScenario
{
    public static void Run()
    {
        ConsoleHelper.WriteSectionHeader("PART 3: DEPENDENCY INJECTION & GLOBAL SERVICES");

        // Setup DI Container
        var services = new ServiceCollection();
        services.AddFinova();
        var serviceProvider = services.BuildServiceProvider();

        RunGlobalTaxIdService(serviceProvider);
        RunGlobalBankAccountService(serviceProvider);
        RunEuropeIbanValidator(serviceProvider);
        RunGlobalAdaptersForEurope(serviceProvider);
    }

    private static void RunGlobalTaxIdService(ServiceProvider provider)
    {
        ConsoleHelper.WriteSubHeader("1", "Global Tax ID Service (ITaxIdService)");
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
        ConsoleHelper.WriteSubHeader("2", "Global Bank Account Service (IBankAccountService)");
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
        ConsoleHelper.WriteSubHeader("3", "Europe IBAN Validator (via DI)");
        var europeValidator = provider.GetRequiredService<EuropeIbanValidator>();

        var europeResult = europeValidator.Validate("BE68539007547034");
        ConsoleHelper.WriteSimpleResult("BE68539007547034", europeResult.IsValid, "Validated via DI (EuropeIbanValidator)");
    }

    private static void RunGlobalAdaptersForEurope(ServiceProvider provider)
    {
        ConsoleHelper.WriteSubHeader("17", "Global Adapters for Europe");

        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("      Demonstrating usage of Global Services (ITaxIdService) for European identifiers:");
        Console.ResetColor();

        var taxIdService = provider.GetRequiredService<ITaxIdService>();
        var bankAccountService = provider.GetRequiredService<IBankAccountService>();

        // 1. Validate French VAT using ITaxIdService
        string frVat = "FR44732829320"; // Valid VAT (BNP Paribas)
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
}
