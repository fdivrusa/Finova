using Finova.Examples.ConsoleApp.Helpers;
using Finova.Examples.ConsoleApp.Scenarios;

namespace Finova.Examples.ConsoleApp;

public class Program
{
    public static void Main(string[] args)
    {
        ConsoleHelper.WriteHeader("FINOVA SDK EXAMPLES");

        StaticValidationScenario.Run();
        BbanValidationScenario.Run();
        NationalIdScenario.Run();
        SecuritiesValidationScenario.Run();
        DependencyInjectionScenario.Run();
        FluentValidationScenario.Run();
        GlobalServicesScenario.Run();
        GlobalExpansionScenario.Run();
        CapabilitiesScenario.Run();

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("  All scenarios completed successfully!");
        Console.ResetColor();
        Console.WriteLine();
    }
}