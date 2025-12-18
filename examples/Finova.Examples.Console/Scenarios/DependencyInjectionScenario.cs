using Finova.Core.Bic;
using Finova.Core.Iban;
using Finova.Core.Identifiers;
using Finova.Core.PaymentCard;
using Finova.Core.PaymentReference;
using Finova.Core.PaymentReference.Internals;
using Finova.Examples.ConsoleApp.Helpers;
using Finova.Extensions;
using Finova.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Finova.Examples.ConsoleApp.Scenarios;

public static class DependencyInjectionScenario
{
    public static void Run()
    {
        ConsoleHelper.WriteSectionHeader("PART 2: DEPENDENCY INJECTION USAGE");

        // Setup DI Container
        var services = new ServiceCollection();
        services.AddFinova();
        var serviceProvider = services.BuildServiceProvider();

        RunBicValidator(serviceProvider);
        RunPaymentCardValidator(serviceProvider);
        RunIbanValidator(serviceProvider);
        RunPaymentReferenceValidator(serviceProvider);
        RunNationalIdValidator(serviceProvider);
    }

    private static void RunBicValidator(ServiceProvider serviceProvider)
    {
        ConsoleHelper.WriteSubHeader("11", "IBicValidator via DI");
        ConsoleHelper.WriteCode("bicValidator.Validate(bic).IsValid");

        var bicValidator = serviceProvider.GetRequiredService<IBicValidator>();
        string[] testBics = ["GEBABEBB", "BNPAFRPP", "NWBKGB2L", "INVALID"];

        foreach (var bic in testBics)
        {
            ConsoleHelper.WriteSimpleResult(bic, bicValidator.Validate(bic).IsValid);
        }

        Console.WriteLine();
    }

    private static void RunPaymentCardValidator(ServiceProvider serviceProvider)
    {
        ConsoleHelper.WriteSubHeader("12", "IPaymentCardValidator via DI");
        ConsoleHelper.WriteCode("cardValidator.ValidateLuhn(card).IsValid");

        var cardValidator = serviceProvider.GetRequiredService<IPaymentCardValidator>();

        var diTestCards = new[] { "4111111111111111", "5500000000000004", "378282246310005", "1234567890123456" };
        foreach (var card in diTestCards)
        {
            bool isValid = cardValidator.ValidateLuhn(card).IsValid;
            var brand = cardValidator.GetBrand(card);
            ConsoleHelper.WriteSimpleResult(card, isValid, brand.ToString());
        }

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("      CVV Validation:");
        Console.ResetColor();
        ConsoleHelper.WriteSimpleResult("Visa CVV '123'", cardValidator.ValidateCvv("123", PaymentCardBrand.Visa).IsValid);
        ConsoleHelper.WriteSimpleResult("Amex CVV '1234'", cardValidator.ValidateCvv("1234", PaymentCardBrand.AmericanExpress).IsValid);
        ConsoleHelper.WriteSimpleResult("Visa CVV '12' (too short)", cardValidator.ValidateCvv("12", PaymentCardBrand.Visa).IsValid);

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("      Expiration Validation:");
        Console.ResetColor();
        ConsoleHelper.WriteSimpleResult("12/2026", cardValidator.ValidateExpiration(12, 2026).IsValid);
        ConsoleHelper.WriteSimpleResult("01/2020 (expired)", cardValidator.ValidateExpiration(1, 2020).IsValid);
        ConsoleHelper.WriteSimpleResult("13/2025 (bad month)", cardValidator.ValidateExpiration(13, 2025).IsValid);

        Console.WriteLine();
    }

    private static void RunIbanValidator(ServiceProvider serviceProvider)
    {
        ConsoleHelper.WriteSubHeader("13", "IIbanValidator via DI (EuropeIbanValidator)");
        ConsoleHelper.WriteCode("ibanValidator.Validate(iban).IsValid");

        var ibanValidator = serviceProvider.GetRequiredService<IIbanValidator>();

        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine($"      Registered validator: {ibanValidator.GetType().Name}");
        Console.ResetColor();

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("      Multi-country validation via single interface:");
        Console.ResetColor();

        var multiCountryIbans = new (string Iban, string Country)[]
        {
            ("BE68539007547034", "Belgium"),
            ("FR7630006000011234567890189", "France"),
            ("DE89370400440532013000", "Germany"),
            ("IT60X0542811101000000123456", "Italy"),
            ("LU280019400644750000", "Luxembourg"),
            ("NL91ABNA0417164300", "Netherlands"),
            ("ES9121000418450200051332", "Spain"),
            ("GB29NWBK60161331926819", "United Kingdom"),
        };

        foreach (var (iban, country) in multiCountryIbans)
        {
            ConsoleHelper.WriteResult(country[..2], iban, ibanValidator.Validate(iban).IsValid);
        }

        Console.WriteLine();
    }

    private static void RunPaymentReferenceValidator(ServiceProvider serviceProvider)
    {
        ConsoleHelper.WriteSubHeader("14", "IPaymentReferenceValidator via DI");
        ConsoleHelper.WriteCode("refValidator.Validate(reference, PaymentReferenceFormat.IsoRf).IsValid");

        var refValidator = serviceProvider.GetRequiredService<IPaymentReferenceValidator>();

        string validRef = IsoReferenceHelper.Generate("PAYMENT123");
        string[] diRefs = [validRef, "RF00INVALID", "NOT_RF_FORMAT"];

        foreach (var reference in diRefs)
        {
            ConsoleHelper.WriteSimpleResult(reference, refValidator.Validate(reference, PaymentReferenceFormat.IsoRf).IsValid);
        }

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("      Validate with specific format (DI):");
        Console.ResetColor();
        ConsoleHelper.WriteCode("refValidator.Validate(reference, format)");

        var formatExamples = new (string Ref, PaymentReferenceFormat Format)[]
        {
            ("+++090/9337/55493+++", PaymentReferenceFormat.LocalBelgian),
            ("+71<123456789012347", PaymentReferenceFormat.LocalDenmark)
        };

        foreach (var (reference, format) in formatExamples)
        {
            var result = refValidator.Validate(reference, format);
            ConsoleHelper.WriteSimpleResult($"{format}", result.IsValid, reference);
        }

        Console.WriteLine();
    }

    private static void RunNationalIdValidator(ServiceProvider serviceProvider)
    {
        ConsoleHelper.WriteSubHeader("15", "INationalIdService via DI");
        ConsoleHelper.WriteCode("nationalIdService.Validate(countryCode, id).IsValid");

        var nationalIdService = serviceProvider.GetRequiredService<INationalIdService>();

        var examples = new (string Country, string Id, bool Expected, string Description)[]
        {
            ("BE", "72020290081", true, "Belgium NN (Valid)"),
            ("BE", "72020290082", false, "Belgium NN (Invalid Checksum)"),
            ("FR", "1 80 01 45 000 000 69", true, "France NIR (Valid)"),
            ("DE", "T22000124", true, "Germany Steuer-ID (Valid)"),
            ("IT", "RSSMRA80A01H501U", true, "Italy CF (Valid)"),
            ("ES", "12345678Z", true, "Spain DNI (Valid)"),
            ("SE", "8112189876", true, "Sweden PN (Valid)"),
            ("GB", "QQ123456A", true, "UK NINO (Valid)"),
            ("NO", "01010012356", true, "Norway Fodselsnummer (Valid)"),
            ("FI", "131052-308T", true, "Finland Henkilotunnus (Valid)")
        };

        foreach (var (country, id, expected, desc) in examples)
        {
            var result = nationalIdService.Validate(country, id);
            ConsoleHelper.WriteSimpleResult($"{desc}", result.IsValid, result.IsValid ? "Valid" : result.Errors.FirstOrDefault()?.Message);
        }

        Console.WriteLine();
    }
}
