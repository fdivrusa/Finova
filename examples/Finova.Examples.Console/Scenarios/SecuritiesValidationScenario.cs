using Finova.Core.Identifiers;
using Finova.Examples.ConsoleApp.Helpers;

namespace Finova.Examples.ConsoleApp.Scenarios;

/// <summary>
/// Demonstrates securities identifier validation including ISIN, CUSIP, SEDOL, and Currency codes.
/// </summary>
public static class SecuritiesValidationScenario
{
    public static void Run()
    {
        ConsoleHelper.WriteSectionHeader("SECURITIES IDENTIFIERS VALIDATION");

        RunIsinValidation();
        RunCusipValidation();
        RunSedolValidation();
        RunCurrencyValidation();
    }

    private static void RunIsinValidation()
    {
        ConsoleHelper.WriteSubHeader("1", "ISIN Validation (ISO 6166)");
        ConsoleHelper.WriteCode("IsinValidator.Validate(isin).IsValid");

        var isins = new (string Description, string Isin)[]
        {
            ("Apple Inc. (US)", "US0378331005"),
            ("Microsoft Corp. (US)", "US5949181045"),
            ("Tesla Inc. (US)", "US88160R1014"),
            ("SAP AG (Germany)", "DE0007164600"),
            ("BNP Paribas (France)", "FR0000131104"),
            ("Toyota Motor (Japan)", "JP3633400001"),
            ("Roche (Switzerland)", "CH0012032048"),
            ("BAE Systems (UK)", "GB0002634946"),
            ("Heineken (Netherlands)", "NL0000009165"),
            ("BHP Group (Australia)", "AU000000BHP4"),
            ("Invalid Checksum", "US0378331006"),
            ("Invalid Format", "1234567890AB"),
        };

        foreach (var (description, isin) in isins)
        {
            var result = IsinValidator.Validate(isin);
            ConsoleHelper.WriteSimpleResult($"{description,-25} {isin}", result.IsValid);
        }

        Console.WriteLine();

        // Demonstrate parsing
        Console.WriteLine("      Parsing ISIN details:");
        var details = IsinValidator.Parse("US0378331005");
        if (details != null)
        {
            Console.WriteLine($"        Country Code: {details.CountryCode}");
            Console.WriteLine($"        NSIN: {details.Nsin}");
            Console.WriteLine($"        Check Digit: {details.CheckDigit}");
            Console.WriteLine($"        Valid: {details.IsValid}");
        }

        Console.WriteLine();

        // Demonstrate generation
        Console.WriteLine("      Generating ISIN:");
        var generated = IsinValidator.Generate("US", "037833100");
        Console.WriteLine($"        Generated ISIN: {generated}");
        Console.WriteLine($"        Validation: {IsinValidator.Validate(generated).IsValid}");

        Console.WriteLine();
    }

    private static void RunCusipValidation()
    {
        ConsoleHelper.WriteSubHeader("2", "CUSIP Validation (US/Canada Securities)");
        ConsoleHelper.WriteCode("CusipValidator.Validate(cusip).IsValid");

        var cusips = new (string Description, string Cusip)[]
        {
            ("Apple Inc.", "037833100"),
            ("Microsoft Corp.", "594918104"),
            ("Berkshire Hathaway", "084670702"),
            ("Amazon.com", "023135106"),
            ("Google (Alphabet A)", "02079K305"),
            ("US Treasury Bond", "912810RZ3"),
            ("Invalid Checksum", "037833101"),
            ("Invalid Length", "12345"),
        };

        foreach (var (description, cusip) in cusips)
        {
            var result = CusipValidator.Validate(cusip);
            ConsoleHelper.WriteSimpleResult($"{description,-25} {cusip}", result.IsValid);
        }

        Console.WriteLine();

        // Demonstrate parsing
        Console.WriteLine("      Parsing CUSIP details:");
        var details = CusipValidator.Parse("037833100");
        if (details != null)
        {
            Console.WriteLine($"        CUSIP: {details.Cusip}");
            Console.WriteLine($"        Issuer Number: {details.IssuerNumber}");
            Console.WriteLine($"        Issue Number: {details.IssueNumber}");
            Console.WriteLine($"        Check Digit: {details.CheckDigit}");
            Console.WriteLine($"        Valid: {details.IsValid}");
        }

        Console.WriteLine();
    }

    private static void RunSedolValidation()
    {
        ConsoleHelper.WriteSubHeader("3", "SEDOL Validation (UK Securities)");
        ConsoleHelper.WriteCode("SedolValidator.Validate(sedol).IsValid");

        var sedols = new (string Description, string Sedol)[]
        {
            ("Vodafone Group", "0263494"),
            ("BP plc", "0798059"),
            ("HSBC Holdings", "0540528"),
            ("GlaxoSmithKline", "0296731"),
            ("AstraZeneca", "0989529"),
            ("Unilever PLC", "B10RZP7"),
            ("Invalid Checksum", "0263495"),
            ("Invalid Length", "12345"),
        };

        foreach (var (description, sedol) in sedols)
        {
            var result = SedolValidator.Validate(sedol);
            ConsoleHelper.WriteSimpleResult($"{description,-25} {sedol}", result.IsValid);
        }

        Console.WriteLine();

        // Demonstrate parsing
        Console.WriteLine("      Parsing SEDOL details:");
        var details = SedolValidator.Parse("0263494");
        if (details != null)
        {
            Console.WriteLine($"        SEDOL: {details.Sedol}");
            Console.WriteLine($"        Base Code: {details.BaseCode}");
            Console.WriteLine($"        Check Digit: {details.CheckDigit}");
            Console.WriteLine($"        Valid: {details.IsValid}");
        }

        Console.WriteLine();
    }

    private static void RunCurrencyValidation()
    {
        ConsoleHelper.WriteSubHeader("4", "Currency Code Validation (ISO 4217)");
        ConsoleHelper.WriteCode("CurrencyValidator.Validate(code).IsValid");

        var currencies = new[]
        {
            "USD", // US Dollar
            "EUR", // Euro
            "GBP", // British Pound
            "JPY", // Japanese Yen
            "CHF", // Swiss Franc
            "CNY", // Chinese Yuan
            "AUD", // Australian Dollar
            "CAD", // Canadian Dollar
            "INR", // Indian Rupee
            "BRL", // Brazilian Real
            "XXX", // Invalid code
            "ABC", // Invalid code
        };

        foreach (var code in currencies)
        {
            var result = CurrencyValidator.Validate(code);
            if (result.IsValid)
            {
                var info = CurrencyValidator.Parse(code);
                ConsoleHelper.WriteSimpleResult($"{code,-8} {info?.Name ?? "Unknown",-30}", true);
            }
            else
            {
                ConsoleHelper.WriteSimpleResult($"{code,-8} Invalid", false);
            }
        }

        Console.WriteLine();

        // Demonstrate getting currency info
        Console.WriteLine("      Currency Details for EUR:");
        var euroInfo = CurrencyValidator.Parse("EUR");
        if (euroInfo != null)
        {
            Console.WriteLine($"        Name: {euroInfo.Name}");
            Console.WriteLine($"        Numeric Code: {euroInfo.NumericCode}");
            Console.WriteLine($"        Minor Units (Decimal Places): {euroInfo.MinorUnits}");
        }

        Console.WriteLine();

        // List some major currencies
        Console.WriteLine("      Major World Currencies:");
        var majorCurrencies = new[] { "USD", "EUR", "GBP", "JPY", "CHF", "CNY", "AUD", "CAD" };
        foreach (var code in majorCurrencies)
        {
            var info = CurrencyValidator.Parse(code);
            if (info != null)
            {
                Console.WriteLine($"        {code}: {info.Name}");
            }
        }

        Console.WriteLine();
    }
}
