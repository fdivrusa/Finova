# Finova

<div align="center">

**The Offline Financial Validation Toolkit for .NET**

*IBAN · Payment References · Cards · VAT · Business Numbers*

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
    - **Country Specific Rules:** Supports specific validation rules for **51 countries** (Belgium, France, Germany, Italy, Spain, UK, Netherlands, etc.).
    - **Generic Validation:** Supports parsing and validating checksums for all ISO-compliant countries.
- **Payment Cards:**
    - **Luhn Algorithm:** Mod 10 validation for PAN numbers.
    - **Brand Detection:** Identifies Visa, Mastercard, Amex, Discover, JCB, Maestro.
    - **Secure CVV Check:** Format-only validation (Safe for PCI-DSS).
- **BIC/SWIFT:** Structural validation (ISO 9362) & Cross-check with IBAN country code.

### 🧾 **Payment References**
- **ISO 11649 (RF):** Generates and validates international `RF` creditor references.
- **Local Formats:**
    - **Belgium:** OGM/VCS (`+++XXX/XXXX/XXXXX+++`)
    - **Finland:** Viitenumero
    - **Norway:** KID
    - **Sweden:** OCR
    - **Switzerland:** QR Reference
    - **Slovenia:** SI12

### 🏢 **Business Numbers**
- **Enterprise Numbers:** Validates Belgian KBO/BCE (Mod97) & French SIRET/SIREN (Luhn).
- **VAT Numbers:** Validates formatting and check digits for EU-27 countries.

### 🔗 **FluentValidation Integration**
- **Extensions:** `MustBeValidIban`, `MustBeValidBic`, `MustBeValidVat`, `MustBeValidPaymentReference`, etc.
- **Seamless:** Integrates directly into your existing `AbstractValidator` classes.

---

## 📦 Installation

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

### 1\. Validate an IBAN

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

### 2\. Validate a Payment Card

```csharp
using Finova.Core.PaymentCard;

// Validates checksum (Luhn) and detects brand
var result = PaymentCardValidator.Validate("4532123456789012");

if (result.IsValid)
{
    Console.WriteLine($"Valid {result.Brand} Card"); // Output: Valid Visa Card
}
```

### 3\. Generate a Payment Reference

```csharp
using Finova.Generators;
using Finova.Core.PaymentReference;

var generator = new PaymentReferenceGenerator();

// Generate Belgian OGM (+++000/0012/34569+++)
string ogm = generator.Generate("123456", PaymentReferenceFormat.LocalBelgian);

// Generate ISO RF (RF89INVOICE2024)
string isoRef = generator.Generate("INVOICE2024", PaymentReferenceFormat.IsoRf);
```

### 4\. FluentValidation Integration

```csharp
using FluentValidation;
using Finova.Extensions.FluentValidation;

public class CustomerValidator : AbstractValidator<Customer>
#### Option A: Dependency Injection (Recommended)

Register Finova in your `Program.cs`:

```csharp
using Finova.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Registers all validators (IBAN, VAT, Enterprise, PaymentReference, etc.)
builder.Services.AddFinova();

var app = builder.Build();
```

Inject and use the validators in your services:

```csharp
using Finova.Core.Vat;
using Finova.Core.Enterprise;

public class BusinessService
{
    private readonly IVatValidator _vatValidator;
    private readonly IEnterpriseValidator _enterpriseValidator;

    public BusinessService(IVatValidator vatValidator, IEnterpriseValidator enterpriseValidator)
    {
        _vatValidator = vatValidator;
        _enterpriseValidator = enterpriseValidator;
    }

    public void RegisterCompany(string vatNumber, string enterpriseNumber)
    {
        // Validates VAT format and checksum for any EU country
        if (!_vatValidator.Validate(vatNumber).IsValid)
        {
            throw new Exception("Invalid VAT Number");
        }

        // Validates Enterprise Number (e.g., SIRET, KBO)
        if (!_enterpriseValidator.Validate(enterpriseNumber).IsValid)
        {
            throw new Exception("Invalid Enterprise Number");
        }
    }
}
```

#### Option B: Static Usage (Simple)

You can also use the static helpers directly without DI:

```csharp
using Finova.Services;
using Finova.Core.Enterprise;

// 1. Validate VAT Number (Auto-detects country)
bool isVatValid = EuropeVatValidator.Validate("FR12345678901").IsValid;

// 2. Validate Enterprise Number (Auto-detects country)
bool isEntValid = EuropeEnterpriseValidator.ValidateEnterpriseNumber("BE0123456789").IsValid;

// 3. Validate Specific Enterprise Type
bool isSiretValid = EuropeEnterpriseValidator.ValidateEnterpriseNumber(
    "73282932000074", 
    EnterpriseNumberType.FranceSiret
).IsValid;
```

-----
using Finova.Countries.Europe.France.Validators;
using Finova.Countries.Europe.Belgium.Validators;

// 1. Get detailed SIRET components (SIREN + NIC)
var details = FranceSiretValidator.GetSiretDetails("73282932000074");
Console.WriteLine($"SIREN: {details.Siren}, NIC: {details.Nic}");

// 2. Format a Belgian Enterprise Number
string formatted = BelgiumEnterpriseValidator.Format("0456789123");
// Output: 0456.789.123
```

-----

# 🗺️ Roadmap

Finova is strictly offline. Future updates focus on schema compliance, developer experience, and mathematical validation.

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
- **Enterprise Numbers:** French SIRET/SIREN, Belgian KBO/BCE  

---

## 🔮 v1.4.0 — National Identifiers *(Planned)*
- **National IDs:** Netherlands KVK, Spain NIF/CIF  
- **Modern Payment Strings:** EPC QR Code payload builder, Swiss QR parsing  

---

## 🔮 v1.5.0 — Global Routing *(Future)*
- **USA:** ABA routing number checksums  
- **Canada:** Transit number validation  
- **Australia:** BSB number validation  

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