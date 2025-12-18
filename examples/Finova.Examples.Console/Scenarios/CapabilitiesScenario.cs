using Finova.Examples.ConsoleApp.Helpers;

namespace Finova.Examples.ConsoleApp.Scenarios;

public static class CapabilitiesScenario
{
    public static void Run()
    {
        Console.WriteLine();
        ConsoleHelper.WriteSectionHeader("SUPPORTED COUNTRIES & CAPABILITIES");
        Console.WriteLine("  Finova supports 51 European countries/territories.");
        Console.WriteLine();
        Console.WriteLine("  Legend: ✓ = Supported, - = Not Applicable/Not Implemented");
        Console.WriteLine();
        Console.WriteLine("  ┌──────────────────────┬──────┬──────┬──────┬──────┬──────────┐");
        Console.WriteLine("  │ Country              │ Code │ IBAN │ VAT  │ TaxID│ Pay. Ref │");
        Console.WriteLine("  ├──────────────────────┼──────┼──────┼──────┼──────┼──────────┤");

        var capabilities = new[]
        {
            ("Albania", "AL", true, true, true, false),
            ("Andorra", "AD", true, true, true, false),
            ("Austria", "AT", true, true, true, false),
            ("Azerbaijan", "AZ", true, true, true, false),
            ("Belarus", "BY", true, true, true, false),
            ("Belgium", "BE", true, true, true, true),
            ("Bosnia & Herz.", "BA", true, true, true, false),
            ("Bulgaria", "BG", true, true, true, false),
            ("Croatia", "HR", true, true, true, false),
            ("Cyprus", "CY", true, true, true, false),
            ("Czech Republic", "CZ", true, true, true, false),
            ("Denmark", "DK", true, true, true, true),
            ("Estonia", "EE", true, true, true, false),
            ("Faroe Islands", "FO", true, true, true, false),
            ("Finland", "FI", true, true, true, true),
            ("France", "FR", true, true, true, false),
            ("Georgia", "GE", true, true, true, false),
            ("Germany", "DE", true, true, true, false),
            ("Gibraltar", "GI", true, true, true, false),
            ("Greece", "GR", true, true, true, false),
            ("Greenland", "GL", true, false, true, false), // No VAT in Greenland
            ("Hungary", "HU", true, true, true, false),
            ("Iceland", "IS", true, true, true, false),
            ("Ireland", "IE", true, true, true, false),
            ("Italy", "IT", true, true, true, true),
            ("Kosovo", "XK", true, false, true, false), // No VAT in Kosovo (uses Fiscal Number)
            ("Latvia", "LV", true, true, true, false),
            ("Liechtenstein", "LI", true, true, true, false),
            ("Lithuania", "LT", true, true, true, false),
            ("Luxembourg", "LU", true, true, true, false),
            ("Malta", "MT", true, true, true, false),
            ("Moldova", "MD", true, true, true, false),
            ("Monaco", "MC", true, true, true, false),
            ("Montenegro", "ME", true, true, true, false),
            ("Netherlands", "NL", true, true, true, false),
            ("North Macedonia", "MK", true, true, true, false),
            ("Norway", "NO", true, true, true, true),
            ("Poland", "PL", true, true, true, false),
            ("Portugal", "PT", true, true, true, true),
            ("Romania", "RO", true, true, true, false),
            ("San Marino", "SM", true, true, true, false),
            ("Serbia", "RS", true, true, true, false),
            ("Slovakia", "SK", true, true, true, false),
            ("Slovenia", "SI", true, true, true, true),
            ("Spain", "ES", true, true, true, false),
            ("Sweden", "SE", true, true, true, true),
            ("Switzerland", "CH", true, true, true, true),
            ("Turkey", "TR", true, true, true, false),
            ("Ukraine", "UA", true, true, true, false),
            ("United Kingdom", "GB", true, true, true, false),
            ("Vatican City", "VA", true, true, true, false),
        };

        foreach (var (name, code, iban, vat, ent, pay) in capabilities)
        {
            string ibanStr = iban ? "  ✓ " : "  - ";
            string vatStr = vat ? "  ✓ " : "  - ";
            string entStr = ent ? "  ✓ " : "  - ";
            string payStr = pay ? "    ✓    " : "    -    ";

            Console.WriteLine($"  │ {name,-20} │ {code,-4} │ {ibanStr} │ {vatStr} │ {entStr} │ {payStr}│");
        }
        Console.WriteLine("  └──────────────────────┴──────┴──────┴──────┴──────┴──────────┘");
        Console.WriteLine();

        ConsoleHelper.WriteSectionHeader("GLOBAL CAPABILITIES (Non-European)");
        Console.WriteLine("  ┌──────────────────────┬──────┬──────────────┬──────────────┐");
        Console.WriteLine("  │ Country              │ Code │ Tax ID / ID  │ Enterprise   │");
        Console.WriteLine("  ├──────────────────────┼──────┼──────────────┼──────────────┤");

        var globalCapabilities = new[]
        {
            ("Australia", "AU", "TFN", "ABN"),
            ("Brazil", "BR", "CPF", "CNPJ"),
            ("Canada", "CA", "SIN", "BN"),
            ("China", "CN", "RIC", "USCC"),
            ("India", "IN", "Aadhaar/PAN", "PAN"),
            ("Japan", "JP", "My Number", "Corp. Num"),
            ("Mexico", "MX", "CURP/RFC", "RFC"),
            ("Singapore", "SG", "NRIC", "UEN"),
            ("United States", "US", "EIN/SSN", "EIN")
        };

        foreach (var (name, code, tax, ent) in globalCapabilities)
        {
            Console.WriteLine($"  │ {name,-20} │ {code,-4} │ {tax,-12} │ {ent,-12} │");
        }
        Console.WriteLine("  └──────────────────────┴──────┴──────────────┴──────────────┘");
        Console.WriteLine();
    }
}
