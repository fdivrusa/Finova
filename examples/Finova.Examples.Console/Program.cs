using Finova.Examples.ConsoleApp.Helpers;
using Finova.Examples.ConsoleApp.Scenarios;

ConsoleHelper.WriteHeader("FINOVA - Financial Validation Examples");

// 1. Static Validation Scenario (High Performance, No DI)
StaticValidationScenario.Run();

// 2. Dependency Injection Scenario (Best Practice for Apps)
DependencyInjectionScenario.Run();

// 3. FluentValidation Scenario (Integration with FluentValidation)
FluentValidationScenario.Run();

// 4. Global Expansion Scenario (Non-European Validators)
GlobalExpansionScenario.Run();

// 5. Global Services Scenario (Tax ID & Bank Account Services)
GlobalServicesScenario.Run();

// 6. BBAN Validation Scenario
BbanValidationScenario.Run();

// 7. National ID Scenario
NationalIdScenario.Run();

// 8. Securities Identifiers Scenario (ISIN, CUSIP, SEDOL, Currency)
SecuritiesValidationScenario.Run();

// 9. Capabilities Overview
CapabilitiesScenario.Run();

Console.WriteLine();
Console.WriteLine("Press any key to exit...");
Console.ReadKey();
