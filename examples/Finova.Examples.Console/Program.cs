using Finova.Belgium.Extensions;
using Finova.Belgium.Validators;
using Finova.Core.Extensions;
using Finova.Core.Interfaces;
using Finova.Core.Models;
using Finova.Core.Validators;
using Finova.France.Extensions;
using Finova.France.Validators;
using Finova.Germany.Extensions;
using Finova.Germany.Validators;
using Finova.Italy.Extensions;
using Finova.Italy.Validators;
using Finova.Netherlands.Extensions;
using Finova.Netherlands.Validators;
using Finova.Spain.Extensions;
using Finova.Spain.Validators;
using Microsoft.Extensions.DependencyInjection;

// ══════════════════════════════════════════════════════════════════════════════
// HELPER METHODS FOR COLORED OUTPUT
// ══════════════════════════════════════════════════════════════════════════════

void WriteHeader(string text)
{
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("╔" + new string('═', 74) + "╗");
    Console.WriteLine("║" + text.PadLeft(37 + text.Length / 2).PadRight(74) + "║");
    Console.WriteLine("╚" + new string('═', 74) + "╝");
    Console.ResetColor();
    Console.WriteLine();
}

void WriteSectionHeader(string text)
{
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("┌" + new string('─', 74) + "┐");
    Console.WriteLine("│" + text.PadLeft(37 + text.Length / 2).PadRight(74) + "│");
    Console.WriteLine("└" + new string('─', 74) + "┘");
    Console.ResetColor();
    Console.WriteLine();
}

void WriteSubHeader(string number, string text)
{
    Console.ForegroundColor = ConsoleColor.Magenta;
    Console.Write($"  ■ {number}. ");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine(text);
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine("  " + new string('─', 50));
    Console.ResetColor();
}

void WriteCountryHeader(string flag, string country)
{
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine($"    {flag} {country}");
    Console.ResetColor();
}

void WriteResult(string label, string value, bool isValid, string? extra = null)
{
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.Write($"      {label}: ");
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write($"{value,-32} ");
    
    if (isValid)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("✓ Valid");
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("✗ Invalid");
    }
    
    if (!string.IsNullOrEmpty(extra))
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write($" [{extra}]");
    }
    
    Console.ResetColor();
    Console.WriteLine();
}

void WriteSimpleResult(string value, bool isValid, string? extra = null)
{
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write($"      {value,-35} ");
    
    if (isValid)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("✓ Valid");
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("✗ Invalid");
    }
    
    if (!string.IsNullOrEmpty(extra))
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write($" [{extra}]");
    }
    
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

void WriteBullet(string text)
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write("      • ");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine(text);
    Console.ResetColor();
}

// ══════════════════════════════════════════════════════════════════════════════
// MAIN PROGRAM
// ══════════════════════════════════════════════════════════════════════════════

Console.Clear();
WriteHeader("FINOVA - Financial Validation Examples");

// ══════════════════════════════════════════════════════════════════════════════
// PART 1: STATIC USAGE
// ══════════════════════════════════════════════════════════════════════════════
WriteSectionHeader("PART 1: STATIC USAGE (Direct, High-Performance)");

// ─────────────────────────────────────────
// 1. BIC/SWIFT Code Validation
// ─────────────────────────────────────────
WriteSubHeader("1", "BIC/SWIFT Code Validation");

string[] bicCodes = [
    "GEBABEBB",      // BNP Paribas Fortis (Belgium)
    "KREDBEBB",      // KBC Bank (Belgium)
    "BNPAFRPP",      // BNP Paribas (France)
    "COBADEFF",      // Commerzbank (Germany)
    "ABNANL2A",      // ABN AMRO (Netherlands)
    "GEBABEBB021",   // With branch code
    "INVALID",       // Invalid
    "ABC"            // Too short
];

foreach (var bic in bicCodes)
{
    bool isValid = BicValidator.IsValid(bic);
    WriteSimpleResult(bic, isValid);
}

Console.WriteLine();

// ─────────────────────────────────────────
// 2. Multi-Country IBAN Validation
// ─────────────────────────────────────────
WriteSubHeader("2", "Multi-Country IBAN Validation");

// Belgium
WriteCountryHeader("🇧🇪", "Belgium");
string[] belgianIbans = ["BE68539007547034", "BE68 5390 0754 7034", "BE00123456789012"];
foreach (var iban in belgianIbans)
{
    WriteSimpleResult(iban, BelgiumIbanValidator.ValidateBelgianIban(iban));
}

// France
WriteCountryHeader("🇫🇷", "France");
string[] frenchIbans = ["FR1420041010050500013M02606", "FR14 2004 1010 0505 0001 3M02 606", "FR0012345678901234567890123"];
foreach (var iban in frenchIbans)
{
    WriteSimpleResult(iban, FranceIbanValidator.ValidateFranceIban(iban));
}

// Germany
WriteCountryHeader("🇩🇪", "Germany");
string[] germanIbans = ["DE89370400440532013000", "DE89 3704 0044 0532 0130 00", "DE00123456789012345678"];
foreach (var iban in germanIbans)
{
    WriteSimpleResult(iban, GermanyIbanValidator.ValidateGermanyIban(iban));
}

// Netherlands
WriteCountryHeader("🇳🇱", "Netherlands");
string[] dutchIbans = ["NL91ABNA0417164300", "NL91 ABNA 0417 1643 00", "NL20INGB0001234567", "NL00XXXX0000000000"];
foreach (var iban in dutchIbans)
{
    WriteSimpleResult(iban, NetherlandsIbanValidator.ValidateDutchIban(iban));
}

// Italy
WriteCountryHeader("🇮🇹", "Italy");
string[] italianIbans = ["IT60X0542811101000000123456", "IT60 X054 2811 1010 0000 0123 456", "IT00X0000000000000000000000"];
foreach (var iban in italianIbans)
{
    WriteSimpleResult(iban, ItalyIbanValidator.ValidateItalyIban(iban));
}

// Spain
WriteCountryHeader("🇪🇸", "Spain");
string[] spanishIbans = ["ES9121000418450200051332", "ES91 2100 0418 4502 0005 1332", "ES0000000000000000000000"];
foreach (var iban in spanishIbans)
{
    WriteSimpleResult(iban, SpainIbanValidator.ValidateSpainIban(iban));
}

Console.WriteLine();

// ─────────────────────────────────────────
// 3. Payment Card Validation
// ─────────────────────────────────────────
WriteSubHeader("3", "Payment Card Validation (Luhn Algorithm)");

var testCards = new (string Number, string Description)[]
{
    ("4111111111111111", "Visa"),
    ("4111 1111 1111 1111", "Visa (spaces)"),
    ("5500000000000004", "Mastercard"),
    ("340000000000009", "Amex"),
    ("6011000000000004", "Discover"),
    ("3530111333300000", "JCB"),
    ("1234567890123456", "Invalid"),
};

foreach (var (number, desc) in testCards)
{
    bool isValid = PaymentCardValidator.IsValidLuhn(number);
    var brand = PaymentCardValidator.GetBrand(number);
    
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.Write($"      {desc,-15} ");
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write($"{number,-22} ");
    
    if (isValid)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("✓ Valid   ");
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("✗ Invalid ");
    }
    
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.WriteLine($"[{brand}]");
    Console.ResetColor();
}

Console.WriteLine();

// ─────────────────────────────────────────
// 4. ISO 11649 Payment Reference (RF)
// ─────────────────────────────────────────
WriteSubHeader("4", "ISO 11649 Payment Reference (RF)");

string[] references = ["RF18539007547034", "RF18 5390 0754 7034", "RF00123456789", "INVALID"];
foreach (var reference in references)
{
    WriteSimpleResult(reference, PaymentReferenceValidator.IsValid(reference));
}

Console.WriteLine();
Console.ForegroundColor = ConsoleColor.DarkCyan;
Console.WriteLine("      Generate RF Reference:");
Console.ResetColor();

try
{
    string generated = PaymentReferenceValidator.Generate("INV2024001234");
    WriteInfo("Input", "INV2024001234");
    WriteInfo("Generated", generated);
    WriteSuccess($"Validation passed");
}
catch (Exception ex)
{
    WriteError(ex.Message);
}

Console.WriteLine();

// ─────────────────────────────────────────
// 5. Belgian VAT & Enterprise Number
// ─────────────────────────────────────────
WriteSubHeader("5", "Belgian VAT & Enterprise Number");

string[] vatNumbers = ["BE0764117795", "BE 0764.117.795", "0764117795", "BE9999999999"];
foreach (var vat in vatNumbers)
{
    WriteResult("VAT", vat, BelgiumVatValidator.IsValid(vat));
}

Console.WriteLine();
Console.ForegroundColor = ConsoleColor.DarkCyan;
Console.WriteLine("      Formatting:");
Console.ResetColor();

try
{
    WriteInfo("VAT", $"0764117795 → {BelgiumVatValidator.Format("0764117795")}");
    WriteInfo("KBO", $"0764117795 → {BelgiumEnterpriseValidator.Format("0764117795")}");
}
catch (ArgumentException ex)
{
    WriteError(ex.Message);
}

// ══════════════════════════════════════════════════════════════════════════════
// PART 2: DEPENDENCY INJECTION USAGE
// ══════════════════════════════════════════════════════════════════════════════
WriteSectionHeader("PART 2: DEPENDENCY INJECTION USAGE");

// Setup DI Container
var services = new ServiceCollection();
services.AddFinovaCoreServices();
services.AddFinovaBelgium();
services.AddFinovaFrance();
services.AddFinovaGermany();
services.AddFinovaNetherlands();
services.AddFinovaItaly();
services.AddFinovaSpain();
var serviceProvider = services.BuildServiceProvider();

// ─────────────────────────────────────────
// 6. IBicValidator via DI
// ─────────────────────────────────────────
WriteSubHeader("6", "IBicValidator via DI");

var bicValidator = serviceProvider.GetRequiredService<IBicValidator>();
string[] testBics = ["GEBABEBB", "BNPAFRPP", "INVALID"];

foreach (var bic in testBics)
{
    WriteSimpleResult(bic, bicValidator.IsValid(bic));
}

Console.WriteLine();

// ─────────────────────────────────────────
// 7. IPaymentCardValidator via DI
// ─────────────────────────────────────────
WriteSubHeader("7", "IPaymentCardValidator via DI");

var cardValidator = serviceProvider.GetRequiredService<IPaymentCardValidator>();

var diTestCards = new[] { "4111111111111111", "5500000000000004", "1234567890123456" };
foreach (var card in diTestCards)
{
    bool isValid = cardValidator.IsValidLuhn(card);
    var brand = cardValidator.GetBrand(card);
    WriteSimpleResult(card, isValid, brand.ToString());
}

Console.WriteLine();
Console.ForegroundColor = ConsoleColor.DarkCyan;
Console.WriteLine("      CVV Validation:");
Console.ResetColor();
WriteSimpleResult("Visa CVV '123'", cardValidator.IsValidCvv("123", PaymentCardBrand.Visa));
WriteSimpleResult("Amex CVV '1234'", cardValidator.IsValidCvv("1234", PaymentCardBrand.AmericanExpress));
WriteSimpleResult("Visa CVV '12' (too short)", cardValidator.IsValidCvv("12", PaymentCardBrand.Visa));

Console.WriteLine();
Console.ForegroundColor = ConsoleColor.DarkCyan;
Console.WriteLine("      Expiration Validation:");
Console.ResetColor();
WriteSimpleResult("12/2026", cardValidator.IsValidExpiration(12, 2026));
WriteSimpleResult("01/2020 (expired)", cardValidator.IsValidExpiration(1, 2020));
WriteSimpleResult("13/2025 (bad month)", cardValidator.IsValidExpiration(13, 2025));

Console.WriteLine();

// ─────────────────────────────────────────
// 8. IIbanValidator via DI (Multi-Country)
// ─────────────────────────────────────────
WriteSubHeader("8", "IIbanValidator via DI (Multi-Country)");

var ibanValidators = serviceProvider.GetServices<IIbanValidator>().ToList();

Console.ForegroundColor = ConsoleColor.DarkCyan;
Console.WriteLine($"      Registered validators: {ibanValidators.Count}");
Console.ResetColor();

foreach (var validator in ibanValidators)
{
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.Write("        → ");
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write(validator.GetType().Name);
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.WriteLine($" ({validator.CountryCode})");
    Console.ResetColor();
}

Console.WriteLine();
Console.ForegroundColor = ConsoleColor.DarkCyan;
Console.WriteLine("      Country-specific validation:");
Console.ResetColor();

var belgiumValidator = serviceProvider.GetRequiredService<BelgiumIbanValidator>();
var franceValidator = serviceProvider.GetRequiredService<FranceIbanValidator>();
var germanyValidator = serviceProvider.GetRequiredService<GermanyIbanValidator>();
var netherlandsValidator = serviceProvider.GetRequiredService<NetherlandsIbanValidator>();
var italyValidator = serviceProvider.GetRequiredService<ItalyIbanValidator>();
var spainValidator = serviceProvider.GetRequiredService<SpainIbanValidator>();

WriteResult("BE", "BE68539007547034", belgiumValidator.IsValidIban("BE68539007547034"));
WriteResult("FR", "FR1420041010050500013M02606", franceValidator.IsValidIban("FR1420041010050500013M02606"));
WriteResult("DE", "DE89370400440532013000", germanyValidator.IsValidIban("DE89370400440532013000"));
WriteResult("NL", "NL91ABNA0417164300", netherlandsValidator.IsValidIban("NL91ABNA0417164300"));
WriteResult("IT", "IT60X0542811101000000123456", italyValidator.IsValidIban("IT60X0542811101000000123456"));
WriteResult("ES", "ES9121000418450200051332", spainValidator.IsValidIban("ES9121000418450200051332"));

Console.WriteLine();

// ─────────────────────────────────────────
// 9. IPaymentReferenceValidator via DI
// ─────────────────────────────────────────
WriteSubHeader("9", "IPaymentReferenceValidator via DI");

var refValidator = serviceProvider.GetRequiredService<IPaymentReferenceValidator>();
string[] diRefs = ["RF18539007547034", "RF00INVALID"];

foreach (var reference in diRefs)
{
    WriteSimpleResult(reference, refValidator.IsValid(reference));
}

// ══════════════════════════════════════════════════════════════════════════════
// SUMMARY
// ══════════════════════════════════════════════════════════════════════════════
Console.WriteLine();
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("  ╔" + new string('═', 72) + "╗");
Console.WriteLine("  ║" + "✅ All examples completed successfully!".PadLeft(45).PadRight(72) + "║");
Console.WriteLine("  ╚" + new string('═', 72) + "╝");
Console.ResetColor();

Console.WriteLine();
Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("  Finova supports:");
Console.ResetColor();

WriteBullet("BIC/SWIFT validation (ISO 9362)");
WriteBullet("IBAN validation for BE, FR, DE, NL, and more");
WriteBullet("Payment card validation (Luhn, brand detection, CVV, expiry)");
WriteBullet("ISO 11649 (RF) payment reference validation & generation");
WriteBullet("Belgian VAT & Enterprise Number validation");
WriteBullet("Full Dependency Injection support");

Console.WriteLine();
Console.ForegroundColor = ConsoleColor.DarkGray;
Console.WriteLine("  " + new string('═', 74));
Console.ResetColor();
Console.WriteLine();
