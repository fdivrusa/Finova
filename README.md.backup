# Finova

<div align="center">

**The Offline Financial Validation Toolkit for .NET### 🆔 **National Identity (New in v1.4.0)**
- **Full European Coverage:** Validates National ID / Citizen ID numbers for **all 51 European countries and territories**.
    - **Western Europe:** Belgium (NN), France (NIR), Germany (Steuer-ID), Netherlands (BSN), UK (NINO), etc.
    - **Northern Europe:** Sweden (Personnummer), Norway (Fødselsnummer), Denmark (CPR), Finland (HETU), Iceland (Kennitala).
    - **Southern Europe:** Italy (Codice Fiscale), Spain (DNI/NIE), Portugal (NIF), Greece (AMKA), Turkey (TC Kimlik).
    - **Eastern Europe:** Poland (PESEL), Romania (CNP), Ukraine (RNTRC), Bulgaria (EGN), etc.
    - **Microstates:** Andorra, Liechtenstein, Monaco, San Marino, Vatican.

### 🌍 **Global Expansion (New in v1.4.0)**AN · Payment References · Cards · VAT · Business Numbers*

[![NuGet Version](https://img.shields.io/nuget/v/Finova.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/Finova/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Finova.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/Finova/)
[![Build Status](https://github.com/fdivrusa/Finova/actions/workflows/ci.yml/badge.svg)](https://github.com/fdivrusa/Finova/actions/workflows/ci.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

*100% Offline | Zero Dependencies | Lightning Fast*

[**Visit the Official Website**](https://finovasharp.com/)

</div>

---

## 🌟 About Finova

**Finova** is a comprehensive **offline** financial validation library for .NET. It allows you to validate financial data (IBANs, Credit Cards, VAT numbers, Payment References) using official checksum algorithms (Luhn, Mod97, ISO 7064) and regex patterns directly on your server.

👉 **Visit the [Official Website](https://finovasharp.com/) for full documentation and feature details.**

### ⚡ Offline Validation Only

> **Important:** Finova performs **100% offline validation**. It does **NOT** contact external services, APIs, or banking networks.

**What Finova Does (Offline):**
- ✅ Validates IBAN format and checksum (ISO 7064 Mod 97)
- ✅ Validates Payment Cards (Luhn Algorithm + Brand Detection)
- ✅ Generates and validates payment references (OGM/VCS, ISO 11649)
- ✅ Validates KBO/BCE and VAT numbers (Syntax + Checksum)
- ✅ Validates BIC/SWIFT Structure (ISO 9362)

**What Finova Does NOT Do:**
- ❌ Does NOT verify if an account/IBAN actually exists at the bank
- ❌ Does NOT perform real-time VIES VAT lookups
- ❌ Does NOT contact external APIs
- ❌ Does NOT require an internet connection

---

## 🚀 Features

### 💳 **Banking & Cards**
Fast, offline regex and checksum validation for European and International formats.
- **IBAN Validation:**
    - **Parsing & Validation:** Extracts country code, check digits, bank code, branch code, and account number.
    - **Country Specific Rules:** Supports specific validation rules for **51 European countries and territories** (Belgium, France, Germany, Italy, Spain, UK, Netherlands, etc.).
    - **Generic Validation:** Supports parsing and validating checksums for all ISO-compliant countries.
- **BBAN Validation (New in v1.4.0):**
    - **Dedicated Validators:** Specific validators for **51 European countries** (e.g., `BelgiumBbanValidator`, `FranceBbanValidator`).
    - **Zero Allocation:** High-performance static methods (`Validate(string)`) for critical paths.
    - **Unified Interface:** `IBbanValidator` for dependency injection scenarios.
- **Payment Cards:**
    - **Luhn Algorithm:** Mod 10 validation for PAN numbers.
    - **Brand Detection:** Identifies Visa, Mastercard, Amex, Discover, JCB, Maestro, RuPay, Mir, Verve, Troy.
    - **Secure CVV Check:** Format-only validation (Safe for PCI-DSS).
- **BIC/SWIFT:** Structural validation (ISO 9362) & Cross-check with IBAN country code.
- **Bank Routing Numbers:** Validates legacy bank codes for major European countries:
    - **Germany:** Bankleitzahl (BLZ)
    - **United Kingdom:** Sort Code
    - **France:** Code Banque
    - **Italy:** ABI Code
    - **Spain:** Código de Entidad

### 🧾 **Payment References**
- **ISO 11649 (RF):** Generates and validates international `RF` creditor references.
- **Local Formats:**
    - **Belgium:** OGM/VCS (`+++XXX/XXXX/XXXXX+++`)
    - **Finland:** Viitenumero
    - **Norway:** KID
    - **Sweden:** OCR
    - **Switzerland:** QR Reference
    - **Slovenia:** SI12
    - **Denmark:** FIK / GIK
    - **Italy:** CBILL / PagoPA
    - **Portugal:** Multibanco

### 🏢 **Business Numbers**
- **Tax IDs:** Validates business registration numbers for **51 European countries** (e.g., Belgian KBO, French SIREN, Italian P.IVA, etc.).
- **VAT Numbers:** Validates formatting and check digits for **European countries** (EU + UK, Norway, Switzerland, etc.).

### � **National Identity (New in v1.6.0)**
- **Full European Coverage:** Validates National ID / Citizen ID numbers for **all 51 European countries and territories**.
    - **Western Europe:** Belgium (NN), France (NIR), Germany (Steuer-ID), Netherlands (BSN), UK (NINO), etc.
    - **Northern Europe:** Sweden (Personnummer), Norway (Fødselsnummer), Denmark (CPR), Finland (HETU), Iceland (Kennitala).
    - **Southern Europe:** Italy (Codice Fiscale), Spain (DNI/NIE), Portugal (NIF), Greece (AMKA), Turkey (TC Kimlik).
    - **Eastern Europe:** Poland (PESEL), Romania (CNP), Ukraine (RNTRC), Bulgaria (EGN), etc.
    - **Microstates:** Andorra, Liechtenstein, Monaco, San Marino, Vatican.

### �🌍 **Global Expansion (New in v1.4.0)**
Finova now supports major economies across North America, South America, Asia, and Oceania.

| Region | Country | Validators |
| :--- | :--- | :--- |
| **North America** | 🇺🇸 USA | Routing Number, EIN |
| | 🇨🇦 Canada | SIN, Business Number (BN), Routing Number |
| **South America** | 🇧🇷 Brazil | CPF, CNPJ, Routing Number |
| | 🇲🇽 Mexico | CURP, RFC |
| **Asia** | 🇨🇳 China | RIC, USCC, CNAPS |
| | 🇯🇵 Japan | My Number, Corporate Number, Bank Account |
| | 🇮🇳 India | Aadhaar, PAN, IFSC |
| | 🇸🇬 Singapore | NRIC, UEN, Bank Account |
| **Oceania** | 🇦🇺 Australia | TFN, ABN, BSB |

> **New in v1.4.0:** Full **Parsing** support (extracting metadata) is now available for:
> - **Routing Numbers:** USA (ABA), Canada (CC), Australia (BSB), India (IFSC), China (CNAPS), Brazil (COMPE).
> - **Bank Accounts:** Japan, Singapore.

<details>
<summary><strong>View Full List of Supported Countries (51)</strong></summary>

| Country | Code | IBAN | VAT | Tax ID | Payment Ref |
| :--- | :---: | :---: | :---: | :---: | :---: |
| Albania | AL | ✅ | ✅ | ✅ | - |
| Andorra | AD | ✅ | ✅ | ✅ | - |
| Austria | AT | ✅ | ✅ | ✅ | - |
| Azerbaijan | AZ | ✅ | ✅ | ✅ | - |
| Belarus | BY | ✅ | ✅ | ✅ | - |
| Belgium | BE | ✅ | ✅ | ✅ | ✅ |
| Bosnia & Herzegovina | BA | ✅ | ✅ | ✅ | - |
| Bulgaria | BG | ✅ | ✅ | ✅ | - |
| Croatia | HR | ✅ | ✅ | ✅ | - |
| Cyprus | CY | ✅ | ✅ | ✅ | - |
| Czech Republic | CZ | ✅ | ✅ | ✅ | - |
| Denmark | DK | ✅ | ✅ | ✅ | ✅ |
| Estonia | EE | ✅ | ✅ | ✅ | - |
| Faroe Islands | FO | ✅ | ✅ | ✅ | - |
| Finland | FI | ✅ | ✅ | ✅ | ✅ |
| France | FR | ✅ | ✅ | ✅ | - |
| Georgia | GE | ✅ | ✅ | ✅ | - |
| Germany | DE | ✅ | ✅ | ✅ | - |
| Gibraltar | GI | ✅ | ✅ | ✅ | - |
| Greece | GR | ✅ | ✅ | ✅ | - |
| Greenland | GL | ✅ | - | ✅ | - |
| Hungary | HU | ✅ | ✅ | ✅ | - |
| Iceland | IS | ✅ | ✅ | ✅ | - |
| Ireland | IE | ✅ | ✅ | ✅ | - |
| Italy | IT | ✅ | ✅ | ✅ | ✅ |
| Kosovo | XK | ✅ | - | ✅ | - |
| Latvia | LV | ✅ | ✅ | ✅ | - |
| Liechtenstein | LI | ✅ | ✅ | ✅ | - |
| Lithuania | LT | ✅ | ✅ | ✅ | - |
| Luxembourg | LU | ✅ | ✅ | ✅ | - |
| Malta | MT | ✅ | ✅ | ✅ | - |
| Moldova | MD | ✅ | ✅ | ✅ | - |
| Monaco | MC | ✅ | ✅ | ✅ | - |
| Montenegro | ME | ✅ | ✅ | ✅ | - |
| Netherlands | NL | ✅ | ✅ | ✅ | - |
| North Macedonia | MK | ✅ | ✅ | ✅ | - |
| Norway | NO | ✅ | ✅ | ✅ | ✅ |
| Poland | PL | ✅ | ✅ | ✅ | - |
| Portugal | PT | ✅ | ✅ | ✅ | ✅ |
| Romania | RO | ✅ | ✅ | ✅ | - |
| San Marino | SM | ✅ | ✅ | ✅ | - |
| Serbia | RS | ✅ | ✅ | ✅ | - |
| Slovakia | SK | ✅ | ✅ | ✅ | - |
| Slovenia | SI | ✅ | ✅ | ✅ | ✅ |
| Spain | ES | ✅ | ✅ | ✅ | - |
| Sweden | SE | ✅ | ✅ | ✅ | ✅ |
| Switzerland | CH | ✅ | ✅ | ✅ | ✅ |
| Turkey | TR | ✅ | ✅ | ✅ | - |
| Ukraine | UA | ✅ | ✅ | ✅ | - |
| United Kingdom | GB | ✅ | ✅ | ✅ | - |
| Vatican City | VA | ✅ | ✅ | ✅ | - |

</details>

### 🔗 **FluentValidation Integration**
- **Extensions:** `MustBeValidIban`, `MustBeValidBic`, `MustBeValidVat`, `MustBeValidPaymentReference`, etc.
- **Seamless:** Integrates directly into your existing `AbstractValidator` classes.

---

## � Dependency Injection & Global Services

Finova provides global composite services that automatically delegate validation to the correct country-specific logic.

```csharp
// 1. Register Finova
services.AddFinova();

// 2. Inject Services
public class MyService(ITaxIdService taxIdService, IBankAccountService bankAccountService)
{
    public void Validate()
    {
        // Validates US EIN
        var result1 = taxIdService.Validate("US", "12-3456789");

        // Validates Singapore Bank Account
        var result2 = bankAccountService.Validate("SG", "1234567890");
    }
}
```

## �📦 Installation

Install via the NuGet Package Manager:

```bash
dotnet add package Finova
```

Or via the Package Manager Console:

```powershell
Install-Package Finova
```

-----

## 📖 Quick Start

### 1. Validate an IBAN

```csharp
using Finova.Services;

// Validates format, checksum, and country-specific rules
// (Does NOT check if account exists)
bool isValid = EuropeIbanValidator.ValidateIban("BE68539007547034").IsValid;

if (isValid)
{
    Console.WriteLine("IBAN structure is valid");
}
```

### 2. Validate a Payment Card

```csharp
using Finova.Core.PaymentCard;

// Validates checksum (Luhn) and detects brand
var result = PaymentCardValidator.Validate("4532123456789012");

if (result.IsValid)
{
    Console.WriteLine($"Valid {result.Brand} Card"); // Output: Valid Visa Card
}
```

### 3. Generate a Payment Reference

```csharp
using Finova.Generators;
using Finova.Core.PaymentReference;

var generator = new PaymentReferenceGenerator();

// Generate Belgian OGM (+++000/0012/34569+++)
string ogm = generator.Generate("123456", PaymentReferenceFormat.LocalBelgian);

// Generate ISO RF (RF89INVOICE2024)
string isoRef = generator.Generate("INVOICE2024", PaymentReferenceFormat.IsoRf);
```

### 4. Global Bank Validation (New in v1.4.0)

```csharp
using Finova.Services;

// Validate US Routing Number (ABA)
var usResult = GlobalBankValidator.ValidateRoutingNumber("US", "121000248");
if (usResult.IsValid) Console.WriteLine("Valid US Routing Number");

// Validate Singapore Bank Account
var sgResult = GlobalBankValidator.ValidateBankAccount("SG", "1234567890");
if (sgResult.IsValid) Console.WriteLine("Valid Singapore Account");
```

### 5. FluentValidation Integration

```csharp
using FluentValidation;
using Finova.Extensions.FluentValidation;

public class CustomerValidator : AbstractValidator<Customer>
{
    public CustomerValidator()
    {
        RuleFor(x => x.Iban).MustBeValidIban();
        RuleFor(x => x.VatNumber).MustBeValidVat();
        RuleFor(x => x.Bic).MustBeValidBic();

        // New in v1.4.0: Global Bank Validation
        RuleFor(x => x.RoutingNumber).MustBeValidBankRoutingNumber(x => x.CountryCode);
        RuleFor(x => x.AccountNumber).MustBeValidBankAccountNumber(x => x.CountryCode);
    }
}
```

#### Option A: Dependency Injection (Recommended)

Register Finova in your `Program.cs`:

```csharp
using Finova.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Registers all validators (IBAN, VAT, TaxID, PaymentReference, etc.)
builder.Services.AddFinova();

var app = builder.Build();
```

Inject and use the validators in your services:

```csharp
using Finova.Core.Vat;
using Finova.Services;

public class BusinessService
{
    private readonly IVatValidator _vatValidator;
    private readonly ITaxIdService _taxIdService;

    public BusinessService(IVatValidator vatValidator, ITaxIdService taxIdService)
    {
        _vatValidator = vatValidator;
        _taxIdService = taxIdService;
    }

    public void RegisterCompany(string countryCode, string vatNumber, string taxId)
    {
        // Validates VAT format and checksum for any EU country
        if (!_vatValidator.Validate(vatNumber).IsValid)
        {
            throw new Exception("Invalid VAT Number");
        }

        // Validates Tax ID (e.g., SIRET, KBO, EIN)
        if (!_taxIdService.Validate(countryCode, taxId).IsValid)
        {
            throw new Exception("Invalid Tax ID");
        }
    }
}
```

#### Option B: Static Usage (Simple)

You can also use the static helpers directly without DI:

```csharp
using Finova.Services;

// 1. Validate VAT Number (Auto-detects country)
bool isVatValid = EuropeVatValidator.ValidateVat("FR12345678901").IsValid;

// 2. Validate Tax ID (Requires Country Code)
bool isTaxIdValid = GlobalIdentityValidator.ValidateTaxId("BE", "0123456789").IsValid;
```

### 5. Tax ID Validator Usage

Finova provides a unified validator for Global Tax IDs (including European Enterprise Numbers), supporting **51+ countries**.

```csharp
using Finova.Services;

// 1. Validate Tax ID
// Validates based on the country code provided (e.g., "DE", "FR", "US")
var result = GlobalIdentityValidator.ValidateTaxId("GB", "12345678");
if (result.IsValid)
{
    Console.WriteLine("Valid UK Tax ID");
}

// 2. Validate Non-European Tax IDs
// Supports global formats like US EIN, Brazil CNPJ, etc.
var usResult = GlobalIdentityValidator.ValidateTaxId("US", "12-3456789");
```

### 6. Advanced Usage (Custom Validators)

For advanced scenarios where you need to implement custom validation logic or use specific algorithms directly, you can use the `ChecksumHelper` class.

> **Note:** This is intended for developers building custom extensions. For standard use cases, prefer the high-level validators.

```csharp
using Finova.Core.Common;

// 1. Validate using Luhn Algorithm (Mod 10)
bool isLuhnValid = ChecksumHelper.ValidateLuhn("79927398713");

// 2. Validate using ISO 7064 Mod 97-10 (IBANs)
bool isMod97Valid = ChecksumHelper.ValidateModulo97("1234567890123456789012345678901");

// 3. Calculate Weighted Modulo 11
int[] weights = { 2, 3, 4, 5, 6, 7 };
int remainder = ChecksumHelper.CalculateWeightedModulo11("123456", weights);
```

-----

## 🗺️ Roadmap

Finova is strictly offline. Future updates focus on schema compliance, developer experience, and mathematical validation.

### Upcoming Releases

#### v1.4.0 - Global Expansion 🌎
- **North America**: Support for USA (Routing Numbers, EIN) and Canada (Business Numbers).
- **Asia**: Support for major Asian economies (Japan, Singapore, etc.).
- **South America**: Support for Brazil (CNPJ/CPF) and others.
- **Oceania**: Support for Australia (TFN, ABN, BSB).
- **National IDs**: Added support for UK (NINO), Sweden, Norway, Finland, Denmark.
- **Architecture**: Introduction of `Finova.Countries.NorthAmerica`, `Finova.Countries.Asia`, etc.
- **Dependency Injection**: Enhanced auto-registration via Assembly Scanning.

#### v1.5.0 - EPC QR Code Support 📱
- **EPC QR String Generation**: Generate the raw payload string for European Payments Council (EPC) QR codes.
- **Zero-Dependency**: Focus on string generation to maintain the "no external dependencies" rule (no image libraries required in core).

---

## ✅ v1.0.0 — Foundation *(Released)*
- Belgian payment references (OGM/VCS)
- ISO 11649 international references
- Comprehensive testing and CI/CD

---

## ✅ v1.1.0 — Core Expansion *(Released)*
- **IBAN Expansion:** Italy (IT) & Spain (ES) specific rules
- **BIC/SWIFT:** Structural format validation (ISO 9362)
- **Payment Cards:** Luhn Algorithm & Brand Detection (Visa/MC/Amex)
- **Reference Validator:** RF Creditor Reference (ISO 11649)

---

## ✅ v1.2.0 — European Unification *(Released)*
- **Finova.Europe:** Unified wrapper package for all SEPA countries
- **Smart Routing:** Auto-detect country rules via `EuropeValidator`
- **Extensions:** FluentValidation integration package (`Finova.Extensions.FluentValidation`)

---

## ✅ v1.3.0 — Corporate Identity *(Released)*
- **VAT Numbers:** EU VAT checksums (VIES offline syntax)
- **Tax IDs:** French SIRET/SIREN, Belgian KBO/BCE

---

## 🔮 v1.4.0 — National Identifiers *(Planned)*
- **National IDs:** Netherlands KVK, Spain NIF/CIF
- **Modern Payment Strings:** EPC QR Code payload builder, Swiss QR parsing
- **USA:** ABA routing number checksums
- **Canada:** Transit number validation
- **Australia:** BSB number validation

---

## 🔮 v1.5.0 — Global Routing *(Future)*
- **EPC QR String Generation**: Generate the raw payload string for European Payments Council (EPC) QR codes.
- **Zero-Dependency**: Focus on string generation to maintain the "no external dependencies" rule (no image libraries required in core).

---

## 🔭 Horizon *(Undetermined)*
- AI-assisted anomaly detection

-----

## 🤝 Contributing

We welcome contributions! Please see [CONTRIBUTING.md](CONTRIBUTING.md) for details.

## 📄 License

This project is licensed under the **MIT License**.

-----

**Made with ❤️ for the .NET Community**

[GitHub](https://github.com/fdivrusa/Finova) • [Issues](https://github.com/fdivrusa/Finova/issues)
