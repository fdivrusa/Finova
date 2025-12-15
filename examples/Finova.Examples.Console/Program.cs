using Finova.Core.Bic;
using Finova.Core.Enterprise;
using Finova.Core.Iban;
using Finova.Core.PaymentCard;
using Finova.Core.PaymentReference;
using Finova.Countries.Europe.Austria.Validators;
using Finova.Countries.Europe.Belgium.Validators;
using Finova.Countries.Europe.France.Validators;
using Finova.Countries.Europe.Germany.Validators;
using Finova.Countries.Europe.Italy.Validators;
using Finova.Countries.Europe.Netherlands.Validators;
using Finova.Countries.Europe.Spain.Validators;
using Finova.Countries.Europe.UnitedKingdom.Validators;
using Finova.Extensions;
using Finova.Extensions.FluentValidation;
using Finova.Services;
using Finova.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
// Alias to avoid conflict between Finova.Core.Common.ValidationResult and FluentValidation.Results.ValidationResult
using FinovaResult = Finova.Core.Common.ValidationResult;

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

    // DEBUG LEI
    if (choice == "99")
    {
        string lei = "5493001KJTIIGC8Y1R17";
        var val = new Finova.Core.Identifiers.LeiValidator();
        var res = val.Validate(lei);
        Console.WriteLine($"LEI: {lei} Valid: {res.IsValid}");

        var sb = new System.Text.StringBuilder();
        foreach (char c in lei)
        {
            if (char.IsDigit(c)) sb.Append(c);
            else sb.Append(c - 'A' + 10);
        }
        var bi = System.Numerics.BigInteger.Parse(sb.ToString());
        Console.WriteLine($"BigInt: {bi}");
        Console.WriteLine($"Mod97: {bi % 97}");
        return;
    }

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
        Console.WriteLine("7. Enterprise Number Validation");
        Console.WriteLine("8. Back to Main Menu");
        Console.WriteLine();
        Console.Write("Enter choice (1-8): ");

        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1": RunIbanValidationExamples(); break;
            case "2": RunIbanParsingExamples(); break;
            case "3": RunBicExamples(); break;
            case "4": RunPaymentReferenceExamples(); break;
            case "5": RunPaymentCardExamples(); break;
            case "6": RunVatExamples(); break;
            case "7": RunEnterpriseExamples(); break;
            case "8": return;
            default: WriteError("Invalid choice."); Console.ReadKey(); break;
        }

        if (choice != "8")
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
    Console.WriteLine("  Belgium OGM:");
    string ogm = "+++090/9337/55493+++";
    WriteSimpleResult(ogm, PaymentReferenceValidator.Validate(ogm, PaymentReferenceFormat.LocalBelgian).IsValid);
}

void RunPaymentCardExamples()
{
    WriteSubHeader("5", "Payment Card Validation");
    string[] cards = ["453201511283036", "INVALID"];
    foreach (var card in cards)
    {
        var result = PaymentCardValidator.Validate(card);
        WriteInfo(card, result.IsValid ? $"Valid ({result.IsValid})" : "Invalid");
    }
}

void RunVatExamples()
{
    WriteSubHeader("6", "VAT Validation");
    string[] vats = ["FR00123456789", "DE123456789"];
    var vatValidator = new EuropeVatValidator();
    foreach (var vat in vats)
    {
        var result = vatValidator.Validate(vat);
        WriteSimpleResult(vat, result.IsValid);
    }
}

void RunEnterpriseExamples()
{
    WriteSubHeader("7", "Enterprise Number Validation");

    // 1. Auto-Detect Country
    WriteInfo("Auto-Detect", "Using EuropeEnterpriseValidator.Validate(number)");
    string[] numbers = ["12002701600357", "0764.117.795"]; // French SIRET, Belgian CBE
    string[] countryCodes = ["FR", "BE"];
    foreach (var num in numbers)
    {
        var result = EuropeEnterpriseValidator.ValidateEnterpriseNumber(num, countryCodes[numbers.ToList().IndexOf(num)]);
        WriteSimpleResult(num, result.IsValid);
        if (result.IsValid)
        {
            // In a real scenario, you might want to know which country/type was detected.
            // The current simple wrapper returns bool/result.
        }
    }

    // 2. Specific Type
    Console.WriteLine();
    WriteInfo("Specific Type", "Using EuropeEnterpriseValidator.Validate(number, type)");
    var kboResult = EuropeEnterpriseValidator.ValidateEnterpriseNumber("0764.117.795", EnterpriseNumberType.BelgiumEnterpriseNumber);
    WriteSimpleResult("0764.117.795 (BE)", kboResult.IsValid);

    // 3. Normalization
    Console.WriteLine();
    WriteInfo("Normalization", "GetNormalizedNumber");
    string raw = "1200 2701 600 357";
    var normalized = EuropeEnterpriseValidator.GetNormalizedNumber(raw);
    Console.WriteLine($"  Raw: '{raw}' -> Norm: '{normalized}'");
}

// ══════════════════════════════════════════════════════════════════════════════
// PART 2: DEPENDENCY INJECTION
// ══════════════════════════════════════════════════════════════════════════════

void RunDiExamples()
{
    // Console.Clear(); // Removed for compatibility
    WriteSectionHeader("PART 2: DEPENDENCY INJECTION");

    // 1. Setup DI Container
    var services = new ServiceCollection();
    services.AddFinova(); // Registers all validators
    var provider = services.BuildServiceProvider();

    // 2. Resolve Services
    // IBAN
    var ibanVal = provider.GetRequiredService<IIbanValidator>();
    WriteInfo("IIbanValidator", "Resolved " + ibanVal.GetType().Name);
    WriteSimpleResult("DE89370400440532013000", ibanVal.Validate("DE89370400440532013000").IsValid);

    // Payment Card
    var cardVal = provider.GetRequiredService<IPaymentCardValidator>();
    WriteInfo("IPaymentCardValidator", "Resolved " + cardVal.GetType().Name);
    WriteSimpleResult("453201511283036", cardVal.Validate("453201511283036").IsValid);

    // Enterprise Validator
    var enterpriseVal = provider.GetRequiredService<IEnterpriseValidator>();
    WriteInfo("IEnterpriseValidator", "Resolved " + enterpriseVal.GetType().Name);
    WriteSimpleResult("BE0456789123", enterpriseVal.Validate("BE0456789123").IsValid);

    // Specific Country Validator (e.g., Belgium)
    var beValidator = provider.GetRequiredService<BelgiumEnterpriseValidator>();
    WriteInfo("BelgiumEnterpriseValidator", "Resolved " + beValidator.GetType().Name);
    WriteSimpleResult("0456.789.123", beValidator.Validate("0456.789.123").IsValid);

    Console.WriteLine();
    Console.WriteLine("Press any key to continue...");
    Console.ReadLine();
}

// ══════════════════════════════════════════════════════════════════════════════
// PART 3: FLUENT VALIDATION
// ══════════════════════════════════════════════════════════════════════════════

void RunFluentValidationExamples()
{
    // Console.Clear(); // Removed for compatibility
    WriteSectionHeader("PART 3: FLUENT VALIDATION");

    var customer = new Customer
    {
        Iban = "INVALID",
        Bic = "ABC",
        CardNumber = "123",
        VatNumber = "XX123",
        EnterpriseNumber = "INVALID_ENT",
        CountryCode = "BE"
    };

    var validator = new CustomerValidator();
    var result = validator.Validate(customer);

    if (!result.IsValid)
    {
        WriteError("Validation Failed:");
        foreach (var error in result.Errors)
        {
            Console.WriteLine($"  - {error.PropertyName}: {error.ErrorMessage}");
        }
    }
    else
    {
        WriteInfo("Success", "Validation Passed");
    }

    // Valid Example
    Console.WriteLine();
    WriteInfo("Valid Data", "Testing with valid data...");
    customer.Iban = "DE89370400440532013000";
    customer.Bic = "GEBABEBB";
    customer.CardNumber = "453201511283036";
    customer.VatNumber = "DE123456789";
    customer.EnterpriseNumber = "0456.789.123"; // Valid BE number
    customer.CountryCode = "BE";

    result = validator.Validate(customer);
    WriteSimpleResult("Customer", result.IsValid);

    Console.WriteLine();
    Console.WriteLine("Press any key to return to Main Menu...");
    Console.ReadLine();
}

// ══════════════════════════════════════════════════════════════════════════════
// HELPERS & MODELS
// ══════════════════════════════════════════════════════════════════════════════

void WriteHeader(string title)
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("╔" + new string('═', title.Length + 4) + "╗");
    Console.WriteLine("║  " + title + "  ║");
    Console.WriteLine("╚" + new string('═', title.Length + 4) + "╝");
    Console.ResetColor();
}

void WriteSectionHeader(string title)
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("════════════════════════════════════════════════════════════════");
    Console.WriteLine(" " + title);
    Console.WriteLine("════════════════════════════════════════════════════════════════");
    Console.ResetColor();
}

void WriteSubHeader(string id, string title)
{
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine($"\n[{id}] {title}");
    Console.WriteLine(new string('-', title.Length + 4));
    Console.ResetColor();
}

void WriteCountryHeader(string flag, string name)
{
    Console.WriteLine($"\n{flag} {name}");
}

void WriteInfo(string label, string value)
{
    Console.Write($"  {label}: ");
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.WriteLine(value);
    Console.ResetColor();
}

void WriteSimpleResult(string input, bool isValid)
{
    Console.Write($"  {input}: ");
    if (isValid)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("VALID");
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("INVALID");
    }
    Console.ResetColor();
}

void WriteError(string message)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(message);
    Console.ResetColor();
}

void CheckIban(Func<string, FinovaResult> validator, string valid, string invalid)
{
    WriteSimpleResult(valid, validator(valid).IsValid);
    WriteSimpleResult(invalid, validator(invalid).IsValid);
}

// Models for FluentValidation
public class Customer
{
    public required string Iban { get; set; }
    public required string Bic { get; set; }
    public required string CardNumber { get; set; }
    public required string VatNumber { get; set; }
    public required string EnterpriseNumber { get; set; }
    public required string CountryCode { get; set; }
}

public class CustomerValidator : AbstractValidator<Customer>
{
    public CustomerValidator()
    {
        RuleFor(x => x.Iban).MustBeValidIban();
        RuleFor(x => x.Bic).MustBeValidBic();
        RuleFor(x => x.CardNumber).MustBeValidPaymentCard();
        RuleFor(x => x.VatNumber).MustBeValidVat();
        RuleFor(x => x.EnterpriseNumber).MustBeValidEnterpriseNumber(x => x.CountryCode);
    }
}

// SepaPaymentRequest for FluentValidation Example
public class SepaPaymentRequest
{
    public required string Iban { get; set; }
    public required string Bic { get; set; }
    public required decimal Amount { get; set; }
    public required string EnterpriseNumber { get; set; }
    public required string CountryCode { get; set; }
}

public class SepaPaymentRequestValidator : AbstractValidator<SepaPaymentRequest>
{
    public SepaPaymentRequestValidator()
    {
        RuleFor(x => x.Iban).MustBeValidIban();
        RuleFor(x => x.Bic).MustBeValidBic().MustMatchIbanCountry(x => x.Iban);
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.EnterpriseNumber).MustBeValidEnterpriseNumber(x => x.CountryCode);
    }
}
