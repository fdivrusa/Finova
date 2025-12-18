using Finova.Countries.Europe.Belgium.Validators;
using Finova.Countries.Europe.France.Validators;
using Finova.Countries.Europe.Germany.Validators;
using Finova.Countries.Europe.Italy.Validators;
using Finova.Countries.Europe.Spain.Validators;
using Finova.Countries.Europe.Sweden.Validators;
using Finova.Countries.Europe.UnitedKingdom.Validators;
using Finova.Examples.ConsoleApp.Helpers;

namespace Finova.Examples.ConsoleApp.Scenarios;

public static class NationalIdScenario
{
    public static void Run()
    {
        ConsoleHelper.WriteSectionHeader("PART 3: NATIONAL ID VALIDATION (New in v1.6.0)");
        Console.WriteLine("Validating National Identity Numbers for various European countries.");
        Console.WriteLine();

        RunBelgium();
        RunFrance();
        RunGermany();
        RunItaly();
        RunSpain();
        RunSweden();
        RunUnitedKingdom();
    }

    private static void RunBelgium()
    {
        ConsoleHelper.WriteSubHeader("3.1", "Belgium (Numéro National / Rijksregisternummer)");
        ConsoleHelper.WriteCode("BelgiumNationalIdValidator.ValidateStatic(nn)");

        var examples = new[]
        {
            "72020290081",       // Valid (Born before 2000)
            "01010100126",       // Valid (Born after 2000)
            "72.02.02-900.81",   // Valid with separators
            "72020290082"        // Invalid checksum
        };

        foreach (var id in examples)
        {
            ConsoleHelper.WriteSimpleResult(id, BelgiumNationalIdValidator.ValidateStatic(id).IsValid);
        }
        Console.WriteLine();
    }

    private static void RunFrance()
    {
        ConsoleHelper.WriteSubHeader("3.2", "France (NIR / Numéro de Sécurité Sociale)");
        ConsoleHelper.WriteCode("FranceNationalIdValidator.ValidateStatic(nir)");

        var examples = new[]
        {
            "1 80 01 45 000 000 69", // Valid Male
            "2 80 01 45 000 000 19", // Valid Female
            "180014500000069",       // Valid No Spaces
            "1 80 01 2A 000 000 92", // Valid Corsica 2A
            "1 80 01 99 000 000 69"  // Invalid Month
        };

        foreach (var id in examples)
        {
            ConsoleHelper.WriteSimpleResult(id, FranceNationalIdValidator.ValidateStatic(id).IsValid);
        }
        Console.WriteLine();
    }

    private static void RunGermany()
    {
        ConsoleHelper.WriteSubHeader("3.3", "Germany (Steuer-ID)");
        ConsoleHelper.WriteCode("GermanyNationalIdValidator.ValidateStatic(id)");

        var examples = new[]
        {
            "T22000124",  // Valid
            "L01X00T44",  // Valid
            "T2200012 4", // Valid with space
            "123456789"   // Invalid format
        };

        foreach (var id in examples)
        {
            ConsoleHelper.WriteSimpleResult(id, GermanyNationalIdValidator.ValidateStatic(id).IsValid);
        }
        Console.WriteLine();
    }

    private static void RunItaly()
    {
        ConsoleHelper.WriteSubHeader("3.4", "Italy (Codice Fiscale)");
        ConsoleHelper.WriteCode("ItalyNationalIdValidator.ValidateStatic(cf)");

        var examples = new[]
        {
            "RSSMRA80A01H501U",     // Valid
            "VRDGLC70M20L736A",     // Valid
            "RSS MRA 80A01 H501 U", // Valid with spaces
            "RSSMRA80A01H501Z"      // Invalid checksum
        };

        foreach (var id in examples)
        {
            ConsoleHelper.WriteSimpleResult(id, ItalyNationalIdValidator.ValidateStatic(id).IsValid);
        }
        Console.WriteLine();
    }

    private static void RunSpain()
    {
        ConsoleHelper.WriteSubHeader("3.5", "Spain (DNI / NIE)");
        ConsoleHelper.WriteCode("SpainNationalIdValidator.ValidateStatic(dni)");

        var examples = new[]
        {
            "12345678Z",   // Valid DNI
            "X1234567L",   // Valid NIE (X)
            "Y1234567X",   // Valid NIE (Y)
            "Z1234567R",   // Valid NIE (Z)
            "12345678A"    // Invalid checksum
        };

        foreach (var id in examples)
        {
            ConsoleHelper.WriteSimpleResult(id, SpainNationalIdValidator.ValidateStatic(id).IsValid);
        }
        Console.WriteLine();
    }

    private static void RunSweden()
    {
        ConsoleHelper.WriteSubHeader("3.6", "Sweden (Personnummer)");
        ConsoleHelper.WriteCode("SwedenNationalIdValidator.ValidateStatic(pn)");

        var examples = new[]
        {
            "8112189876",    // Valid 10-digit
            "198112189876",  // Valid 12-digit
            "811218-9876",   // Valid with dash
            "8112189877"     // Invalid checksum
        };

        foreach (var id in examples)
        {
            ConsoleHelper.WriteSimpleResult(id, SwedenNationalIdValidator.ValidateStatic(id).IsValid);
        }
        Console.WriteLine();
    }

    private static void RunUnitedKingdom()
    {
        ConsoleHelper.WriteSubHeader("3.7", "United Kingdom (NINO)");
        ConsoleHelper.WriteCode("UnitedKingdomNationalIdValidator.ValidateStatic(nino)");

        var examples = new[]
        {
            "QQ123456A", // Valid
            "AA123456A", // Valid
            "GB123456A", // Invalid prefix
            "QQ123456"   // Invalid length
        };

        foreach (var id in examples)
        {
            ConsoleHelper.WriteSimpleResult(id, UnitedKingdomNationalIdValidator.ValidateStatic(id).IsValid);
        }
        Console.WriteLine();
    }
}
