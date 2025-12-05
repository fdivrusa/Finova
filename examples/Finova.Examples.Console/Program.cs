using Finova.Belgium.Services;
using Finova.Belgium.Validators;
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
using Finova.Countries.Europe.Netherlands.Validators;
using Finova.Countries.Europe.Norway.Validators;
using Finova.Countries.Europe.Poland.Validators;
using Finova.Countries.Europe.Portugal.Validators;
using Finova.Countries.Europe.Romania.Validators;
using Finova.Countries.Europe.Serbia.Validators;
using Finova.Countries.Europe.Spain.Validators;
using Finova.Countries.Europe.Sweden.Validators;
using Finova.Countries.Europe.UnitedKingdom.Validators;
using Finova.Core.Interfaces;
using Finova.Core.Models;
using Finova.Core.Validators;
using Finova.Extensions;
using Finova.Extensions.FluentValidation;
using Finova.Services;
using FluentValidation;
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
WriteSubHeader("1", "BIC/SWIFT Code Validation (ISO 9362)");

string[] bicCodes =
[
    "GEBABEBB",       // BNP Paribas Fortis (Belgium)
    "KREDBEBB",       // KBC Bank (Belgium)
    "BNPAFRPP",       // BNP Paribas (France)
    "COBADEFF",       // Commerzbank (Germany)
    "ABNANL2A",       // ABN AMRO (Netherlands)
    "BCITITMM",       // Intesa Sanpaolo (Italy)
    "BABORULU",       // Banque Raiffeisen (Luxembourg) - Invalid country code!
    "BGLLLULL",       // Banque de Luxembourg
    "CABORUBEXXX",    // Invalid - RU country code mismatch
    "NWBKGB2L",       // NatWest (United Kingdom)
    "CAIXESBB",       // CaixaBank (Spain)
    "GEBABEBB021",    // With branch code (11 chars)
    "INVALID",        // Invalid
    "ABC"             // Too short
];

foreach (var bic in bicCodes)
{
    bool isValid = BicValidator.Validate(bic);
    WriteSimpleResult(bic, isValid);
}

Console.WriteLine();

// ─────────────────────────────────────────
// 2. Multi-Country IBAN Validation (21 Countries)
// ─────────────────────────────────────────
WriteSubHeader("2", "Multi-Country IBAN Validation (21 Countries)");

// Austria 🇦🇹
WriteCountryHeader("🇦🇹", "Austria (AT - 20 characters)");
string[] austrianIbans = ["AT611904300234573201", "AT61 1904 3002 3457 3201", "AT483200000012345864", "AT00123456789012345678"];
foreach (var iban in austrianIbans)
{
    WriteSimpleResult(iban, AustriaIbanValidator.ValidateAustriaIban(iban));
}

// Belgium 🇧🇪
WriteCountryHeader("🇧🇪", "Belgium (BE - 16 characters)");
string[] belgianIbans = ["BE68539007547034", "BE68 5390 0754 7034", "BE71096123456769", "BE00123456789012"];
foreach (var iban in belgianIbans)
{
    WriteSimpleResult(iban, BelgiumIbanValidator.ValidateBelgiumIban(iban));
}

// Czech Republic 🇨🇿
WriteCountryHeader("🇨🇿", "Czech Republic (CZ - 24 characters, Modulo 11)");
string[] czechIbans = ["CZ6508000000192000145399", "CZ65 0800 0000 1920 0014 5399", "CZ9455000000001011038930", "CZ0012345678901234567890"];
foreach (var iban in czechIbans)
{
    WriteSimpleResult(iban, CzechRepublicIbanValidator.ValidateCzechIban(iban));
}

// Denmark 🇩🇰
WriteCountryHeader("🇩🇰", "Denmark (DK - 18 characters)");
string[] danishIbans = ["DK5000400440116243", "DK50 0040 0440 1162 43", "DK0012345678901234"];
foreach (var iban in danishIbans)
{
    WriteSimpleResult(iban, DenmarkIbanValidator.ValidateDenmarkIban(iban));
}

// France 🇫🇷
WriteCountryHeader("🇫🇷", "France (FR - 27 characters)");
string[] frenchIbans = ["FR7630006000011234567890189", "FR14 2004 1010 0505 0001 3M02 606", "FR0012345678901234567890123"];
foreach (var iban in frenchIbans)
{
    WriteSimpleResult(iban, FranceIbanValidator.ValidateFranceIban(iban));
}

// Germany 🇩🇪
WriteCountryHeader("🇩🇪", "Germany (DE - 22 characters)");
string[] germanIbans = ["DE89370400440532013000", "DE89 3704 0044 0532 0130 00", "DE00123456789012345678"];
foreach (var iban in germanIbans)
{
    WriteSimpleResult(iban, GermanyIbanValidator.ValidateGermanyIban(iban));
}

// Italy 🇮🇹
WriteCountryHeader("🇮🇹", "Italy (IT - 27 characters)");
string[] italianIbans = ["IT60X0542811101000000123456", "IT60 X054 2811 1010 0000 0123 456", "IT00X0000000000000000000000"];
foreach (var iban in italianIbans)
{
    WriteSimpleResult(iban, ItalyIbanValidator.ValidateItalyIban(iban));
}

// Luxembourg 🇱🇺
WriteCountryHeader("🇱🇺", "Luxembourg (LU - 20 characters)");
string[] luxembourgIbans = ["LU280019400644750000", "LU28 0019 4006 4475 0000", "LU120010001234567891", "LU00000000000000000"];
foreach (var iban in luxembourgIbans)
{
    WriteSimpleResult(iban, LuxembourgIbanValidator.ValidateLuxembourgIban(iban));
}

// Netherlands 🇳🇱
WriteCountryHeader("🇳🇱", "Netherlands (NL - 18 characters)");
string[] dutchIbans = ["NL91ABNA0417164300", "NL91 ABNA 0417 1643 00", "NL39RABO0300065264", "NL00XXXX0000000000"];
foreach (var iban in dutchIbans)
{
    WriteSimpleResult(iban, NetherlandsIbanValidator.ValidateNetherlandsIban(iban));
}

// Norway 🇳🇴
WriteCountryHeader("🇳🇴", "Norway (NO - 15 characters, Modulo 11)");
string[] norwegianIbans = ["NO9386011117947", "NO93 8601 1117 947", "NO0012345678901"];
foreach (var iban in norwegianIbans)
{
    WriteSimpleResult(iban, NorwayIbanValidator.ValidateNorwayIban(iban));
}

// Poland 🇵🇱
WriteCountryHeader("🇵🇱", "Poland (PL - 28 characters)");
string[] polishIbans = ["PL61109010140000071219812874", "PL61 1090 1014 0000 0712 1981 2874", "PL27114020040000300201355387", "PL00123456789012345678901234"];
foreach (var iban in polishIbans)
{
    WriteSimpleResult(iban, PolandIbanValidator.ValidatePolandIban(iban));
}

// Romania 🇷🇴
WriteCountryHeader("🇷🇴", "Romania (RO - 24 characters, alphanumeric)");
string[] romanianIbans = ["RO49AAAA1B31007593840000", "RO49 AAAA 1B31 0075 9384 0000", "RO66BACX0000001234567890", "RO0012345678901234567890"];
foreach (var iban in romanianIbans)
{
    WriteSimpleResult(iban, RomaniaIbanValidator.ValidateRomaniaIban(iban));
}

// Serbia 🇷🇸
WriteCountryHeader("🇷🇸", "Serbia (RS - 22 characters)");
string[] serbianIbans = ["RS35260005601001611379", "RS35 2600 0560 1001 6113 79", "RS00123456789012345678"];
foreach (var iban in serbianIbans)
{
    WriteSimpleResult(iban, SerbiaIbanValidator.ValidateSerbiaIban(iban));
}

// Spain 🇪🇸
WriteCountryHeader("🇪🇸", "Spain (ES - 24 characters)");
string[] spanishIbans = ["ES9121000418450200051332", "ES91 2100 0418 4502 0005 1332", "ES7921000813610123456789", "ES0000000000000000000000"];
foreach (var iban in spanishIbans)
{
    WriteSimpleResult(iban, SpainIbanValidator.ValidateSpainIban(iban));
}

// United Kingdom 🇬🇧
WriteCountryHeader("🇬🇧", "United Kingdom (GB - 22 characters)");
string[] ukIbans = ["GB29NWBK60161331926819", "GB29 NWBK 6016 1331 9268 19", "GB82WEST12345698765432", "GB00XXXX00000000000000"];
foreach (var iban in ukIbans)
{
    WriteSimpleResult(iban, UnitedKingdomIbanValidator.ValidateUnitedKingdomIban(iban));
}

// Sweden 🇸🇪
WriteCountryHeader("🇸🇪", "Sweden (SE - 24 characters)");
string[] swedishIbans = ["SE4550000000058398257466", "SE45 5000 0000 0583 9825 7466", "SE6412000000012170145230", "SE00123456789012345678901"];
foreach (var iban in swedishIbans)
{
    WriteSimpleResult(iban, SwedenIbanValidator.ValidateSwedenIban(iban));
}

// Finland 🇫🇮
WriteCountryHeader("🇫🇮", "Finland (FI - 18 characters)");
string[] finnishIbans = ["FI2112345600000785", "FI21 1234 5600 0007 85", "FI1410093000123458", "FI0012345678901234"];
foreach (var iban in finnishIbans)
{
    WriteSimpleResult(iban, FinlandIbanValidator.ValidateFinlandIban(iban));
}

// Greece 🇬🇷
WriteCountryHeader("🇬🇷", "Greece (GR - 27 characters)");
string[] greekIbans = ["GR1601101250000000012300695", "GR16 0110 1250 0000 0001 2300 695", "GR9608100010000001234567890", "GR0012345678901234567890123"];
foreach (var iban in greekIbans)
{
    WriteSimpleResult(iban, GreeceIbanValidator.ValidateGreeceIban(iban));
}

// Hungary 🇭🇺
WriteCountryHeader("🇭🇺", "Hungary (HU - 28 characters)");
string[] hungarianIbans = ["HU42117730161111101800000000", "HU42 1177 3016 1111 1018 0000 0000", "HU00123456789012345678901234"];
foreach (var iban in hungarianIbans)
{
    WriteSimpleResult(iban, HungaryIbanValidator.ValidateHungaryIban(iban));
}

// Ireland 🇮🇪
WriteCountryHeader("🇮🇪", "Ireland (IE - 22 characters)");
string[] irishIbans = ["IE29AIBK93115212345678", "IE29 AIBK 9311 5212 3456 78", "IE64IRCE92050112345678", "IE00XXXX00000000000000"];
foreach (var iban in irishIbans)
{
    WriteSimpleResult(iban, IrelandIbanValidator.ValidateIrelandIban(iban));
}

// Portugal 🇵🇹
WriteCountryHeader("🇵🇹", "Portugal (PT - 25 characters, NIB check required)");
// Portuguese IBANs require valid NIB check digits (last 2 digits of BBAN)
// NIB key = 98 - (19-digit-body mod 97) - see strict NIB validation
// Demo: All examples below intentionally fail NIB check to show validation strictness
string[] portugueseIbans = ["PT50000000000000000000197", "PT50 0000 0000 0000 0000 001 97", "PT00XXXX12345678901234567"];
foreach (var iban in portugueseIbans)
{
    WriteSimpleResult(iban, PortugalIbanValidator.ValidatePortugalIban(iban));
}

Console.WriteLine();

// ─────────────────────────────────────────
// 3. EuropeIbanValidator (Auto-Detection)
// ─────────────────────────────────────────
WriteSubHeader("3", "EuropeIbanValidator (Auto-Detects Country)");

string[] europeanIbans =
[
    "AT611904300234573201",       // Austria
    "BE68539007547034",           // Belgium
    "CZ6508000000192000145399",   // Czech Republic
    "DK5000400440116243",         // Denmark
    "FI2112345600000785",         // Finland
    "FR7630006000011234567890189", // France
    "DE89370400440532013000",     // Germany
    "GR1601101250000000012300695", // Greece
    "HU42117730161111101800000000", // Hungary
    "IE29AIBK93115212345678",     // Ireland
    "IT60X0542811101000000123456", // Italy
    "LU280019400644750000",       // Luxembourg
    "NL91ABNA0417164300",         // Netherlands
    "NO9386011117947",            // Norway
    "PL61109010140000071219812874", // Poland
    "RO49AAAA1B31007593840000",   // Romania
    "RS35260005601001611379",     // Serbia
    "ES9121000418450200051332",   // Spain
    "SE4550000000058398257466",   // Sweden
    "GB29NWBK60161331926819",     // United Kingdom
    "XX00123456789012"            // Unknown country (will fail)
];

foreach (var iban in europeanIbans)
{
    bool isValid = EuropeIbanValidator.Validate(iban);
    string country = iban.Length >= 2 ? iban[..2] : "??";
    WriteSimpleResult(iban, isValid, country);
}

Console.WriteLine();

// ─────────────────────────────────────────
// 4. Payment Card Validation
// ─────────────────────────────────────────
WriteSubHeader("4", "Payment Card Validation (Luhn Algorithm)");

var testCards = new (string Number, string Description)[]
{
    ("4111111111111111", "Visa"),
    ("4111 1111 1111 1111", "Visa (spaces)"),
    ("4532015112830366", "Visa"),
    ("5500000000000004", "Mastercard"),
    ("5425233430109903", "Mastercard"),
    ("340000000000009", "Amex"),
    ("378282246310005", "Amex"),
    ("6011000000000004", "Discover"),
    ("3530111333300000", "JCB"),
    ("36000000000008", "Diners Club"),
    ("1234567890123456", "Invalid"),
};

foreach (var (number, desc) in testCards)
{
    bool isValid = PaymentCardValidator.Validate(number);
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
// 5. ISO 11649 Payment Reference (RF)
// ─────────────────────────────────────────
WriteSubHeader("5", "ISO 11649 Payment Reference (RF)");

// Generate valid RF references for testing
string validRf1 = PaymentReferenceValidator.Generate("539007547034");
string validRf2 = PaymentReferenceValidator.Generate("INV2024001234");
string validRf3 = PaymentReferenceValidator.Generate("ABC123");

string[] references = [validRf1, validRf2, validRf3, "RF00123456789", "INVALID"];
foreach (var reference in references)
{
    WriteSimpleResult(reference, PaymentReferenceValidator.IsValid(reference));
}

Console.WriteLine();
Console.ForegroundColor = ConsoleColor.DarkCyan;
Console.WriteLine("      Generate RF Reference from arbitrary input:");
Console.ResetColor();

try
{
    string generated = PaymentReferenceValidator.Generate("INVOICE2024ABC");
    WriteInfo("Input", "INVOICE2024ABC");
    WriteInfo("Generated", generated);
    WriteSuccess($"Validation passed: {PaymentReferenceValidator.IsValid(generated)}");
}
catch (Exception ex)
{
    WriteError(ex.Message);
}

Console.WriteLine();

// ─────────────────────────────────────────
// 6. Belgian Structured Payment Reference (OGM/VCS)
// ─────────────────────────────────────────
WriteSubHeader("6", "Belgian Structured Payment Reference (OGM/VCS)");

Console.ForegroundColor = ConsoleColor.DarkCyan;
Console.WriteLine("      Generate Belgian OGM from communication number:");
Console.ResetColor();

try
{
    var belgiumRefService = new BelgiumPaymentReferenceService();

    // Generate from different inputs (max 10 digits for OGM)
    string ogm1 = belgiumRefService.Generate("1234567890");
    string ogm2 = belgiumRefService.Generate("1");
    string ogm3 = belgiumRefService.Generate("9999999999");

    WriteInfo("Input 1234567890", $"→ {ogm1}");
    WriteInfo("Input 0000000001", $"→ {ogm2}");
    WriteInfo("Input 9999999999", $"→ {ogm3}");

    // Validate
    WriteSimpleResult($"Validate: {ogm1}", belgiumRefService.IsValid(ogm1));
}
catch (Exception ex)
{
    WriteError(ex.Message);
}

Console.WriteLine();

// ─────────────────────────────────────────
// 7. Belgian VAT & Enterprise Number
// ─────────────────────────────────────────
WriteSubHeader("7", "Belgian VAT & Enterprise Number");

string[] vatNumbers = ["BE0764117795", "BE 0764.117.795", "0764117795", "BE0123456789", "BE9999999999"];
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
services.AddFinova();
var serviceProvider = services.BuildServiceProvider();

// ─────────────────────────────────────────
// 8. IBicValidator via DI
// ─────────────────────────────────────────
WriteSubHeader("8", "IBicValidator via DI");

var bicValidator = serviceProvider.GetRequiredService<IBicValidator>();
string[] testBics = ["GEBABEBB", "BNPAFRPP", "NWBKGB2L", "INVALID"];

foreach (var bic in testBics)
{
    WriteSimpleResult(bic, bicValidator.IsValid(bic));
}

Console.WriteLine();

// ─────────────────────────────────────────
// 9. IPaymentCardValidator via DI
// ─────────────────────────────────────────
WriteSubHeader("9", "IPaymentCardValidator via DI");

var cardValidator = serviceProvider.GetRequiredService<IPaymentCardValidator>();

var diTestCards = new[] { "4111111111111111", "5500000000000004", "378282246310005", "1234567890123456" };
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
// 10. IIbanValidator via DI (EuropeIbanValidator)
// ─────────────────────────────────────────
WriteSubHeader("10", "IIbanValidator via DI (EuropeIbanValidator)");

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
    WriteResult(country[..2], iban, ibanValidator.IsValidIban(iban));
}

Console.WriteLine();

// ─────────────────────────────────────────
// 11. IPaymentReferenceValidator via DI
// ─────────────────────────────────────────
WriteSubHeader("11", "IPaymentReferenceValidator via DI");

var refValidator = serviceProvider.GetRequiredService<IPaymentReferenceValidator>();

string validRef = refValidator.Generate("PAYMENT123");
string[] diRefs = [validRef, "RF00INVALID", "NOT_RF_FORMAT"];

foreach (var reference in diRefs)
{
    WriteSimpleResult(reference, refValidator.IsValid(reference));
}

// ══════════════════════════════════════════════════════════════════════════════
// PART 3: FLUENTVALIDATION INTEGRATION
// ══════════════════════════════════════════════════════════════════════════════
WriteSectionHeader("PART 3: FLUENTVALIDATION INTEGRATION");

// ─────────────────────────────────────────
// 12. FluentValidation Extension Methods Overview
// ─────────────────────────────────────────
WriteSubHeader("12", "FluentValidation Extension Methods (Finova.Extensions.FluentValidation)");

Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("      Available extension methods from FinovaValidators:");
Console.ResetColor();
Console.WriteLine();

Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("      ┌─────────────────────────────────────────────────────────────────────┐");
Console.WriteLine("      │  Extension Method              │  Purpose                           │");
Console.WriteLine("      ├─────────────────────────────────────────────────────────────────────┤");
Console.WriteLine("      │  .MustBeValidIban()            │  Validates IBAN (any EU country)   │");
Console.WriteLine("      │  .MustBeValidBic()             │  Validates BIC/SWIFT code          │");
Console.WriteLine("      │  .MustBeValidPaymentCard()     │  Validates card (Luhn algorithm)   │");
Console.WriteLine("      │  .MustMatchIbanCountry(iban)   │  BIC country must match IBAN       │");
Console.WriteLine("      └─────────────────────────────────────────────────────────────────────┘");
Console.ResetColor();
Console.WriteLine();

// Demo: MustBeValidIban()
Console.ForegroundColor = ConsoleColor.Magenta;
Console.WriteLine("      ► MustBeValidIban() - Validates IBAN format and checksum");
Console.ResetColor();

var ibanDemoValidator = new InlineValidator<string>();
ibanDemoValidator.RuleFor(x => x).MustBeValidIban().WithMessage("Invalid IBAN");

string[] ibanTestCases = ["BE68539007547034", "FR7630006000011234567890189", "INVALID123"];
foreach (var iban in ibanTestCases)
{
    var result = ibanDemoValidator.Validate(iban);
    WriteSimpleResult(iban, result.IsValid);
}
Console.WriteLine();

// Demo: MustBeValidBic()
Console.ForegroundColor = ConsoleColor.Magenta;
Console.WriteLine("      ► MustBeValidBic() - Validates BIC/SWIFT code format");
Console.ResetColor();

var bicDemoValidator = new InlineValidator<string>();
bicDemoValidator.RuleFor(x => x).MustBeValidBic().WithMessage("Invalid BIC");

string[] bicTestCases = ["KREDBEBB", "COBADEFF", "NWBKGB2L", "INVALID"];
foreach (var bic in bicTestCases)
{
    var result = bicDemoValidator.Validate(bic);
    WriteSimpleResult(bic, result.IsValid);
}
Console.WriteLine();

// Demo: MustBeValidPaymentCard()
Console.ForegroundColor = ConsoleColor.Magenta;
Console.WriteLine("      ► MustBeValidPaymentCard() - Validates card using Luhn algorithm");
Console.ResetColor();

var cardDemoValidator = new InlineValidator<string>();
cardDemoValidator.RuleFor(x => x).MustBeValidPaymentCard().WithMessage("Invalid card");

string[] cardTestCases = ["4111111111111111", "5500000000000004", "1234567890123456"];
foreach (var card in cardTestCases)
{
    var result = cardDemoValidator.Validate(card);
    WriteSimpleResult(card, result.IsValid);
}
Console.WriteLine();

// Demo: MustMatchIbanCountry()
Console.ForegroundColor = ConsoleColor.Magenta;
Console.WriteLine("      ► MustMatchIbanCountry(iban) - Validates BIC country matches IBAN country");
Console.ResetColor();

var bicCountryTests = new[]
{
    new { Bic = "KREDBEBB", Iban = "BE68539007547034", Desc = "Belgian BIC + Belgian IBAN" },
    new { Bic = "COBADEFF", Iban = "DE89370400440532013000", Desc = "German BIC + German IBAN" },
    new { Bic = "KREDBEBB", Iban = "DE89370400440532013000", Desc = "Belgian BIC + German IBAN (mismatch!)" }
};

foreach (var test in bicCountryTests)
{
    var bicCountryValidator = new InlineValidator<(string Bic, string Iban)>();
    bicCountryValidator.RuleFor(x => x.Bic)
        .MustMatchIbanCountry(x => x.Iban)
        .WithMessage("BIC country must match IBAN country");

    var result = bicCountryValidator.Validate((test.Bic, test.Iban));
    var status = result.IsValid ? "✓" : "✗";
    var color = result.IsValid ? ConsoleColor.Green : ConsoleColor.Red;
    Console.ForegroundColor = color;
    Console.WriteLine($"      {status} {test.Desc}");
    Console.ResetColor();
}

Console.WriteLine();

// ─────────────────────────────────────────
// 13. FluentValidation - SEPA Payment Example
// ─────────────────────────────────────────
WriteSubHeader("13", "FluentValidation - SEPA Payment Example");

Console.ForegroundColor = ConsoleColor.DarkCyan;
Console.WriteLine("      Using validators in a real-world SEPA payment scenario:");
Console.ResetColor();

// Define a sample payment request
var validPayment = new SepaPaymentRequest
{
    DebtorIban = "BE68539007547034",
    DebtorBic = "KREDBEBB",
    CreditorIban = "DE89370400440532013000",
    CreditorBic = "COBADEFF",
    Amount = 1500.00m,
    Currency = "EUR"
};

var invalidPayment = new SepaPaymentRequest
{
    DebtorIban = "INVALID_IBAN",
    DebtorBic = "BAD",
    CreditorIban = "XX00000000000000",
    CreditorBic = "TOOLONGBICCODE",
    Amount = -100m,
    Currency = "USD"
};

var sepaValidator = new SepaPaymentRequestValidator();

var validResult = sepaValidator.Validate(validPayment);
var invalidResult = sepaValidator.Validate(invalidPayment);

Console.WriteLine();
WriteInfo("Valid Payment", $"IsValid = {validResult.IsValid}");
if (validResult.IsValid)
{
    WriteSuccess("All validation rules passed!");
}

Console.WriteLine();
WriteInfo("Invalid Payment", $"IsValid = {invalidResult.IsValid}");
if (!invalidResult.IsValid)
{
    foreach (var error in invalidResult.Errors)
    {
        WriteError($"{error.PropertyName}: {error.ErrorMessage}");
    }
}

Console.WriteLine();

// ─────────────────────────────────────────
// 14. FluentValidation - Card Payment
// ─────────────────────────────────────────
WriteSubHeader("14", "FluentValidation - Card Payment");

var validCard = new CardPaymentRequest
{
    CardNumber = "4532015112830366",
    CardholderName = "John Doe",
    ExpiryMonth = "12",
    ExpiryYear = "28",
    Cvv = "123",
    Amount = 99.99m
};

var invalidCard = new CardPaymentRequest
{
    CardNumber = "1234567890123456",
    CardholderName = "",
    ExpiryMonth = "13",
    ExpiryYear = "20",
    Cvv = "12",
    Amount = 0m
};

var cardPaymentValidator = new CardPaymentRequestValidator();

var validCardResult = cardPaymentValidator.Validate(validCard);
var invalidCardResult = cardPaymentValidator.Validate(invalidCard);

WriteInfo("Valid Card", $"IsValid = {validCardResult.IsValid}");
if (validCardResult.IsValid)
{
    WriteSuccess("Card payment validation passed!");
}

Console.WriteLine();
WriteInfo("Invalid Card", $"IsValid = {invalidCardResult.IsValid}");
if (!invalidCardResult.IsValid)
{
    foreach (var error in invalidCardResult.Errors.Take(5))
    {
        WriteError($"{error.PropertyName}: {error.ErrorMessage}");
    }
}

Console.WriteLine();

// ─────────────────────────────────────────
// 15. FluentValidation - BIC/IBAN Consistency
// ─────────────────────────────────────────
WriteSubHeader("15", "FluentValidation - BIC/IBAN Country Consistency");

Console.ForegroundColor = ConsoleColor.DarkCyan;
Console.WriteLine("      Testing BIC country matches IBAN country:");
Console.ResetColor();

var consistentTransfer = new InternationalTransfer
{
    SenderIban = "BE68539007547034",
    SenderBic = "KREDBEBB",      // Belgian BIC matches Belgian IBAN
    RecipientIban = "DE89370400440532013000",
    RecipientBic = "COBADEFF"    // German BIC matches German IBAN
};

var inconsistentTransfer = new InternationalTransfer
{
    SenderIban = "BE68539007547034",
    SenderBic = "COBADEFF",      // German BIC doesn't match Belgian IBAN!
    RecipientIban = "DE89370400440532013000",
    RecipientBic = "KREDBEBB"    // Belgian BIC doesn't match German IBAN!
};

var transferValidator = new InternationalTransferValidator();

var consistentResult = transferValidator.Validate(consistentTransfer);
var inconsistentResult = transferValidator.Validate(inconsistentTransfer);

Console.WriteLine();
WriteInfo("Consistent (BE→DE)", $"IsValid = {consistentResult.IsValid}");
if (consistentResult.IsValid)
{
    WriteSuccess("BIC countries match IBAN countries!");
}

Console.WriteLine();
WriteInfo("Inconsistent", $"IsValid = {inconsistentResult.IsValid}");
if (!inconsistentResult.IsValid)
{
    foreach (var error in inconsistentResult.Errors)
    {
        WriteError($"{error.PropertyName}: {error.ErrorMessage}");
    }
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
Console.WriteLine("  Finova supports 21 European countries:");
Console.ResetColor();

WriteBullet("[AT] Austria - IBAN validation");
WriteBullet("[BE] Belgium - IBAN, VAT, Enterprise Number, OGM Payment Reference");
WriteBullet("[CZ] Czech Republic - IBAN validation with Modulo 11 check");
WriteBullet("[DK] Denmark - IBAN validation");
WriteBullet("[FI] Finland - IBAN validation");
WriteBullet("[FR] France - IBAN validation with RIB key check");
WriteBullet("[DE] Germany - IBAN validation");
WriteBullet("[GR] Greece - IBAN validation");
WriteBullet("[HU] Hungary - IBAN validation");
WriteBullet("[IE] Ireland - IBAN validation");
WriteBullet("[IT] Italy - IBAN validation with CIN check");
WriteBullet("[LU] Luxembourg - IBAN validation");
WriteBullet("[NL] Netherlands - IBAN validation with Elfproef");
WriteBullet("[NO] Norway - IBAN validation with Modulo 11 check");
WriteBullet("[PL] Poland - IBAN validation");
WriteBullet("[PT] Portugal - IBAN validation with NIB check");
WriteBullet("[RO] Romania - IBAN validation");
WriteBullet("[RS] Serbia - IBAN validation");
WriteBullet("[ES] Spain - IBAN validation with DC check");
WriteBullet("[SE] Sweden - IBAN validation");
WriteBullet("[GB] United Kingdom - IBAN validation");

Console.WriteLine();
Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("  Core validators:");
Console.ResetColor();

WriteBullet("BIC/SWIFT validation (ISO 9362)");
WriteBullet("Payment card validation (Luhn, brand detection, CVV, expiry)");
WriteBullet("ISO 11649 (RF) payment reference validation & generation");
WriteBullet("EuropeIbanValidator - auto-detects and validates any supported country");

Console.WriteLine();
Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("  Integration:");
Console.ResetColor();

WriteBullet("Full Dependency Injection support via AddFinova()");
WriteBullet("FluentValidation extensions: MustBeValidIban(), MustBeValidBic(), MustBeValidPaymentCard(), MustMatchIbanCountry()");

Console.WriteLine();
Console.ForegroundColor = ConsoleColor.DarkGray;
Console.WriteLine("  " + new string('═', 74));
Console.ResetColor();
Console.WriteLine();

// ══════════════════════════════════════════════════════════════════════════════
// FLUENT VALIDATION MODEL CLASSES
// ══════════════════════════════════════════════════════════════════════════════

public class SepaPaymentRequest
{
    public string DebtorIban { get; set; } = string.Empty;
    public string DebtorBic { get; set; } = string.Empty;
    public string CreditorIban { get; set; } = string.Empty;
    public string CreditorBic { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "EUR";
}

public class SepaPaymentRequestValidator : AbstractValidator<SepaPaymentRequest>
{
    public SepaPaymentRequestValidator()
    {
        RuleFor(x => x.DebtorIban)
            .NotEmpty().WithMessage("Debtor IBAN is required")
            .MustBeValidIban().WithMessage("Debtor IBAN is invalid");

        RuleFor(x => x.DebtorBic)
            .NotEmpty().WithMessage("Debtor BIC is required")
            .MustBeValidBic().WithMessage("Debtor BIC is invalid");

        RuleFor(x => x.CreditorIban)
            .NotEmpty().WithMessage("Creditor IBAN is required")
            .MustBeValidIban().WithMessage("Creditor IBAN is invalid");

        RuleFor(x => x.CreditorBic)
            .NotEmpty().WithMessage("Creditor BIC is required")
            .MustBeValidBic().WithMessage("Creditor BIC is invalid");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be positive");

        RuleFor(x => x.Currency)
            .Equal("EUR").WithMessage("SEPA payments must be in EUR");
    }
}

public class CardPaymentRequest
{
    public string CardNumber { get; set; } = string.Empty;
    public string CardholderName { get; set; } = string.Empty;
    public string ExpiryMonth { get; set; } = string.Empty;
    public string ExpiryYear { get; set; } = string.Empty;
    public string Cvv { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

public class CardPaymentRequestValidator : AbstractValidator<CardPaymentRequest>
{
    public CardPaymentRequestValidator()
    {
        RuleFor(x => x.CardNumber)
            .NotEmpty().WithMessage("Card number is required")
            .MustBeValidPaymentCard().WithMessage("Card number is invalid");

        RuleFor(x => x.CardholderName)
            .NotEmpty().WithMessage("Cardholder name is required");

        RuleFor(x => x.ExpiryMonth)
            .NotEmpty().WithMessage("Expiry month is required")
            .Matches(@"^(0[1-9]|1[0-2])$").WithMessage("Invalid expiry month");

        RuleFor(x => x.ExpiryYear)
            .NotEmpty().WithMessage("Expiry year is required")
            .Matches(@"^\d{2}$").WithMessage("Invalid expiry year format");

        RuleFor(x => x.Cvv)
            .NotEmpty().WithMessage("CVV is required")
            .Matches(@"^\d{3,4}$").WithMessage("CVV must be 3 or 4 digits");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be positive");
    }
}

public class InternationalTransfer
{
    public string SenderIban { get; set; } = string.Empty;
    public string SenderBic { get; set; } = string.Empty;
    public string RecipientIban { get; set; } = string.Empty;
    public string RecipientBic { get; set; } = string.Empty;
}

public class InternationalTransferValidator : AbstractValidator<InternationalTransfer>
{
    public InternationalTransferValidator()
    {
        RuleFor(x => x.SenderIban)
            .NotEmpty().WithMessage("Sender IBAN is required")
            .MustBeValidIban().WithMessage("Sender IBAN is invalid");

        RuleFor(x => x.SenderBic)
            .NotEmpty().WithMessage("Sender BIC is required")
            .MustBeValidBic().WithMessage("Sender BIC is invalid")
            .MustMatchIbanCountry(x => x.SenderIban).WithMessage("Sender BIC country must match sender IBAN country");

        RuleFor(x => x.RecipientIban)
            .NotEmpty().WithMessage("Recipient IBAN is required")
            .MustBeValidIban().WithMessage("Recipient IBAN is invalid");

        RuleFor(x => x.RecipientBic)
            .NotEmpty().WithMessage("Recipient BIC is required")
            .MustBeValidBic().WithMessage("Recipient BIC is invalid")
            .MustMatchIbanCountry(x => x.RecipientIban).WithMessage("Recipient BIC country must match recipient IBAN country");
    }
}

