using Finova.Countries.Europe.Austria.Validators;
using Finova.Countries.Europe.Belgium.Validators;
using Finova.Countries.Europe.CzechRepublic.Validators;
using Finova.Countries.Europe.Denmark.Validators;
using Finova.Countries.Europe.Finland.Validators;
using Finova.Countries.Europe.France.Validators;
using Finova.Countries.Europe.Germany.Validators;
using Finova.Countries.Europe.Greece.Validators;
using Finova.Countries.Europe.Hungary.Validators;
using Finova.Countries.Europe.Ireland.Validators;
using Finova.Countries.Europe.Italy.Validators;
using Finova.Countries.Europe.Luxembourg.Validators;
using Finova.Countries.Europe.Monaco.Validators;
using Finova.Countries.Europe.Montenegro.Validators;
using Finova.Countries.Europe.Netherlands.Validators;
using Finova.Countries.Europe.Norway.Validators;
using Finova.Countries.Europe.Poland.Validators;
using Finova.Countries.Europe.Portugal.Validators;
using Finova.Countries.Europe.Romania.Validators;
using Finova.Countries.Europe.Serbia.Validators;
using Finova.Countries.Europe.Spain.Validators;
using Finova.Countries.Europe.Sweden.Validators;
using Finova.Countries.Europe.UnitedKingdom.Validators;
using Finova.Core.Iban;
using Finova.Core.Bic;
using Finova.Core.PaymentCard;
using Finova.Core.PaymentReference;
using Finova.Core.Vat;
using Finova.Core.Common;
using Finova.Extensions;
using Finova.Extensions.FluentValidation;
using Finova.Services;
using Finova.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

// Alias to avoid conflict between Finova.Core.Common.ValidationResult and FluentValidation.Results.ValidationResult
using FinovaResult = Finova.Core.Common.ValidationResult;
using FluentResult = FluentValidation.Results.ValidationResult;
using Finova.Generators;

// ══════════════════════════════════════════════════════════════════════════════
// FINOVA EXAMPLES CONSOLE
// ══════════════════════════════════════════════════════════════════════════════

while (true)
{
    // Console.Clear(); // Removed for compatibility
    WriteHeader("FINOVA - Financial Validation Examples");

    Console.WriteLine("Select an option:");
    Console.WriteLine("1. Static Validation Examples (Direct Usage)");
    Console.WriteLine("2. Dependency Injection Examples");
    Console.WriteLine("3. FluentValidation Integration Examples");
    Console.WriteLine("4. Exit");
    Console.WriteLine();
    Console.Write("Enter choice (1-4): ");

    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            RunStaticExamples();
            break;
        case "2":
            RunDiExamples();
            break;
        case "3":
            RunFluentValidationExamples();
            break;
        case "4":
            return;
        default:
            WriteError("Invalid choice. Press any key to try again.");
            Console.ReadLine();
            break;
    }
}

// ══════════════════════════════════════════════════════════════════════════════
// PART 1: STATIC EXAMPLES
// ══════════════════════════════════════════════════════════════════════════════

void RunStaticExamples()
{
    while (true)
    {
        // Console.Clear(); // Removed for compatibility
        WriteSectionHeader("PART 1: STATIC USAGE");

        Console.WriteLine("Select a category:");
        Console.WriteLine("1. IBAN Validation (Multi-Country)");
        Console.WriteLine("2. IBAN Parsing (Extract Details)");
        Console.WriteLine("3. BIC/SWIFT Validation");
        Console.WriteLine("4. Payment Reference (RF, Local)");
        Console.WriteLine("5. Payment Card Validation");
        Console.WriteLine("6. VAT Validation");
        Console.WriteLine("7. Back to Main Menu");
        Console.WriteLine();
        Console.Write("Enter choice (1-7): ");

        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1": RunIbanValidationExamples(); break;
            case "2": RunIbanParsingExamples(); break;
            case "3": RunBicExamples(); break;
            case "4": RunPaymentReferenceExamples(); break;
            case "5": RunPaymentCardExamples(); break;
            case "6": RunVatExamples(); break;
            case "7": return;
            default: WriteError("Invalid choice."); Console.ReadKey(); break;
        }

        if (choice != "7")
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }
    }
}

void RunIbanValidationExamples()
{
    WriteSubHeader("1", "Multi-Country IBAN Validation");

    // Austria 🇦🇹
    WriteCountryHeader("🇦🇹", "Austria (AT)");
    CheckIban(AustriaIbanValidator.ValidateAustriaIban, "AT611904300234573201", "AT00123456789012345678");

    // Belgium 🇧🇪
    WriteCountryHeader("🇧🇪", "Belgium (BE)");
    CheckIban(BelgiumIbanValidator.ValidateBelgiumIban, "BE68539007547034", "BE00123456789012");

    // France 🇫🇷
    WriteCountryHeader("🇫🇷", "France (FR)");
    CheckIban(FranceIbanValidator.ValidateFranceIban, "FR7630006000011234567890189", "FR0012345678901234567890123");

    // Germany 🇩🇪
    WriteCountryHeader("🇩🇪", "Germany (DE)");
    CheckIban(GermanyIbanValidator.ValidateGermanyIban, "DE89370400440532013000", "DE00123456789012345678");

    // Italy 🇮🇹
    WriteCountryHeader("🇮🇹", "Italy (IT)");
    CheckIban(ItalyIbanValidator.ValidateItalyIban, "IT60X0542811101000000123456", "IT00X0000000000000000000000");

    // Netherlands 🇳🇱
    WriteCountryHeader("🇳🇱", "Netherlands (NL)");
    CheckIban(NetherlandsIbanValidator.ValidateNetherlandsIban, "NL91ABNA0417164300", "NL00XXXX0000000000");

    // Spain 🇪🇸
    WriteCountryHeader("🇪🇸", "Spain (ES)");
    CheckIban(SpainIbanValidator.ValidateSpainIban, "ES9121000418450200051332", "ES0000000000000000000000");

    // United Kingdom 🇬🇧
    WriteCountryHeader("🇬🇧", "United Kingdom (GB)");
    CheckIban(UnitedKingdomIbanValidator.ValidateUnitedKingdomIban, "GB29NWBK60161331926819", "GB00XXXX00000000000000");

    // Auto-Detection
    WriteSubHeader("Auto", "EuropeIbanValidator (Auto-Detect)");
    string[] mixedIbans = ["AT611904300234573201", "BE68539007547034", "XX00123456789012"];
    foreach (var iban in mixedIbans)
    {
        WriteSimpleResult(iban, EuropeIbanValidator.ValidateIban(iban).IsValid);
    }
}

void RunIbanParsingExamples()
{
    WriteSubHeader("2", "IBAN Parsing (Extract Details)");

    // Using EuropeIbanParser
    var parser = new EuropeIbanParser();

    string[] ibansToParse =
    [
        "DE89370400440532013000", // Germany
        "FR7630006000011234567890189", // France
        "BE68539007547034" // Belgium
    ];

    foreach (var iban in ibansToParse)
    {
        var details = parser.ParseIban(iban);
        if (details != null)
        {
            Console.WriteLine($"  Parsing {iban}:");
            WriteInfo("Country", details.CountryCode ?? "N/A");
            WriteInfo("Bank Code", details.BankCode ?? "N/A");
            WriteInfo("Branch Code", details.BranchCode ?? "N/A");
            WriteInfo("Account", details.AccountNumber ?? "N/A");
            WriteInfo("Check Digits", details.CheckDigits);
            Console.WriteLine();
        }
        else
        {
            WriteError($"Failed to parse {iban}");
        }
    }
}

void RunBicExamples()
{
    WriteSubHeader("3", "BIC/SWIFT Validation");
    string[] bics = ["GEBABEBB", "BNPAFRPP", "INVALID", "ABC"];
    foreach (var bic in bics)
    {
        WriteSimpleResult(bic, BicValidator.Validate(bic).IsValid);
    }
}

void RunPaymentReferenceExamples()
{
    WriteSubHeader("4", "Payment Reference (RF & Local)");

    // ISO RF
    Console.WriteLine("  ISO 11649 (RF):");
    var generator = new IsoPaymentReferenceGenerator();
    string rf = generator.Generate("123456");
    WriteInfo("Generated", rf);
    WriteSimpleResult(rf, PaymentReferenceValidator.Validate(rf).IsValid);

    // Belgium OGM
    Console.WriteLine("\n  Belgium (OGM):");

    var beServiceGenerator = new PaymentReferenceGenerator();
    string ogm = beServiceGenerator.Generate("1234567890");
    WriteInfo("Generated", ogm);
    WriteSimpleResult(ogm, PaymentReferenceValidator.Validate(ogm, PaymentReferenceFormat.LocalBelgian).IsValid);
}

void RunPaymentCardExamples()
{
    WriteSubHeader("5", "Payment Card Validation");
    string[] cards = ["4111111111111111", "5500000000000004", "1234567890123456"];
    foreach (var card in cards)
    {
        var isValid = PaymentCardValidator.Validate(card).IsValid;
        var brand = PaymentCardValidator.GetBrand(card);
        WriteSimpleResult(card, isValid, brand.ToString());
    }
}

void RunVatExamples()
{
    WriteSubHeader("6", "VAT Validation");

    // Belgium
    WriteSimpleResult("BE0764117795", BelgiumVatValidator.Validate("BE0764117795").IsValid, "Belgium");

    // Monaco
    WriteSimpleResult("FR00000000000", MonacoVatValidator.Validate("FR00000000000").IsValid, "Monaco");

    // Montenegro
    WriteSimpleResult("02000005", MontenegroVatValidator.Validate("02000005").IsValid, "Montenegro");

    // Generic EuropeVatValidator
    var vatValidator = new EuropeVatValidator();

    string[] vats = ["BE0764117795", "FR00123456789", "INVALID"];
    foreach (var vat in vats)
    {
        var result = vatValidator.Validate(vat);
        WriteSimpleResult(vat, result.IsValid, result.IsValid ? "Valid Format" : "Invalid");
    }
}

// ══════════════════════════════════════════════════════════════════════════════
// PART 2: DI EXAMPLES
// ══════════════════════════════════════════════════════════════════════════════

void RunDiExamples()
{
    // Console.Clear(); // Removed for compatibility
    WriteSectionHeader("PART 2: DEPENDENCY INJECTION");

    var services = new ServiceCollection();
    services.AddFinova();
    var provider = services.BuildServiceProvider();

    // IBAN Validator
    var ibanVal = provider.GetRequiredService<IIbanValidator>();
    WriteInfo("IIbanValidator", "Resolved " + ibanVal.GetType().Name);
    WriteSimpleResult("BE68539007547034", ibanVal.Validate("BE68539007547034").IsValid);

    // BIC Validator
    var bicVal = provider.GetRequiredService<IBicValidator>();
    WriteInfo("IBicValidator", "Resolved " + bicVal.GetType().Name);
    WriteSimpleResult("GEBABEBB", bicVal.Validate("GEBABEBB").IsValid);

    // Payment Reference Validator
    var refVal = provider.GetRequiredService<IPaymentReferenceValidator>();
    WriteInfo("IPaymentReferenceValidator", "Resolved " + refVal.GetType().Name);
    WriteSimpleResult("RF18539007547034", refVal.Validate("RF18539007547034").IsValid);

    Console.WriteLine();
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
}

// ══════════════════════════════════════════════════════════════════════════════
// PART 3: FLUENT VALIDATION EXAMPLES
// ══════════════════════════════════════════════════════════════════════════════

void RunFluentValidationExamples()
{
    // Console.Clear(); // Removed for compatibility
    WriteSectionHeader("PART 3: FLUENT VALIDATION");

    var validator = new SepaPaymentRequestValidator();
    var request = new SepaPaymentRequest
    {
        DebtorIban = "INVALID",
        DebtorBic = "BAD",
        CreditorIban = "DE89370400440532013000",
        CreditorBic = "COBADEFF",
        Amount = 100,
        Currency = "EUR",
        VatNumber = "FR44732829320", // Valid French VAT
        PaymentReference = "RF18539007547034" // Valid RF
    };

    var result = validator.Validate(request);

    WriteInfo("Validating SEPA Request", "...");
    if (!result.IsValid)
    {
        foreach (var error in result.Errors)
        {
            WriteError($"{error.PropertyName}: {error.ErrorMessage}");
        }
    }
    else
    {
        WriteSuccess("Validation passed!");
    }

    //Display data that is valid
    WriteInfo("Debtor IBAN", request.DebtorIban);
    WriteInfo("Creditor IBAN", request.CreditorIban);
    WriteInfo("Debtor BIC", request.DebtorBic);
    WriteInfo("Creditor BIC", request.CreditorBic);
    WriteInfo("VAT Number", request.VatNumber);
    WriteInfo("Payment Ref", request.PaymentReference);

    Console.WriteLine();
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
}

// ══════════════════════════════════════════════════════════════════════════════
// HELPERS & MODELS
// ══════════════════════════════════════════════════════════════════════════════

void CheckIban(Func<string, FinovaResult> validator, string valid, string invalid)
{
    WriteSimpleResult(valid, validator(valid).IsValid);
    WriteSimpleResult(invalid, validator(invalid).IsValid);
}

void WriteHeader(string text)
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("╔" + new string('═', 74) + "╗");
    Console.WriteLine("║" + text.PadLeft(37 + text.Length / 2).PadRight(74) + "║");
    Console.WriteLine("╚" + new string('═', 74) + "╝");
    Console.ResetColor();
}

void WriteSectionHeader(string text)
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("┌" + new string('─', 74) + "┐");
    Console.WriteLine("│" + text.PadLeft(37 + text.Length / 2).PadRight(74) + "│");
    Console.WriteLine("└" + new string('─', 74) + "┘");
    Console.ResetColor();
}

void WriteSubHeader(string number, string text)
{
    Console.ForegroundColor = ConsoleColor.Magenta;
    Console.WriteLine($"\n  ■ {number}. {text}");
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine("  " + new string('─', 50));
    Console.ResetColor();
}

void WriteCountryHeader(string flag, string country)
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine($"    {flag} {country}");
    Console.ResetColor();
}

void WriteSimpleResult(string value, bool isValid, string? extra = null)
{
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write($"      {value,-35} ");
    if (isValid) { Console.ForegroundColor = ConsoleColor.Green; Console.Write("✓ Valid"); }
    else { Console.ForegroundColor = ConsoleColor.Red; Console.Write("✗ Invalid"); }
    if (extra != null) { Console.ForegroundColor = ConsoleColor.DarkYellow; Console.Write($" [{extra}]"); }
    Console.ResetColor();
    Console.WriteLine();
}

void WriteInfo(string label, string value)
{
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.Write($"      {label}: ");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine(value);
    Console.ResetColor();
}

void WriteSuccess(string message)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"      ✓ {message}");
    Console.ResetColor();
}

void WriteError(string message)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"      ✗ {message}");
    Console.ResetColor();
}

public class SepaPaymentRequest
{
    public string DebtorIban { get; set; } = string.Empty;
    public string DebtorBic { get; set; } = string.Empty;
    public string CreditorIban { get; set; } = string.Empty;
    public string CreditorBic { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "EUR";
    public string VatNumber { get; set; } = string.Empty;
    public string PaymentReference { get; set; } = string.Empty;
}

public class SepaPaymentRequestValidator : AbstractValidator<SepaPaymentRequest>
{
    public SepaPaymentRequestValidator()
    {
        RuleFor(x => x.DebtorIban).MustBeValidIban();
        RuleFor(x => x.DebtorBic).MustBeValidBic();
        RuleFor(x => x.CreditorIban).MustBeValidIban();
        RuleFor(x => x.CreditorBic).MustBeValidBic();
        RuleFor(x => x.VatNumber).MustBeValidVat();
        RuleFor(x => x.PaymentReference).MustBeValidPaymentReference();
    }
}
