# Finova

<div align="center">

**The Offline Financial Validation Toolkit for .NET**

*IBAN · Payment References · Cards · VAT · Business Numbers*

[![NuGet Version](https://img.shields.io/nuget/v/Finova.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/Finova/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Finova.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/Finova/)
[![Build Status](https://github.com/fdivrusa/Finova/actions/workflows/ci.yml/badge.svg)](https://github.com/fdivrusa/Finova/actions/workflows/ci.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

**🇧🇪 Belgium · 🇳🇱 Netherlands · 🇱🇺 Luxembourg · 🇫🇷 France · 🇩🇪 Germany · 🇬🇧 UK**

*100% Offline | Zero Dependencies | Lightning Fast*

[**Visit the Official Website**](https://finovapackage.netlify.app/)

</div>

---

## 🌟 About Finova

**Finova** is a comprehensive **offline** financial validation library for .NET. It allows you to validate financial data (IBANs, Credit Cards, VAT numbers, Payment References) using official checksum algorithms (Luhn, Mod97, ISO 7064) and regex patterns directly on your server.

👉 **Visit the [Official Website](https://finovapackage.netlify.app/) for full documentation and feature details.**

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
    - **Belgium (BE):** Format + Mod97 + Bank Code extraction.
    - **Netherlands (NL):** Format + Mod97 + Bank Code (4-letter).
    - **France (FR):** Format + Mod97 + RIB Key + Bank Code.
    - **Germany (DE):** Format + Mod97 + BLZ extraction.
    - **UK (GB):** Format + Mod97 + Sort Code extraction.
    - **Generic:** Supports parsing and validating checksums for all ISO-compliant countries.
- **Payment Cards:**
    - **Luhn Algorithm:** Mod 10 validation for PAN numbers.
    - **Brand Detection:** Identifies Visa, Mastercard, Amex, Discover.
    - **Secure CVV Check:** Format-only validation (Safe for PCI-DSS).
- **BIC/SWIFT:** Structural validation (ISO 9362) & Cross-check with IBAN country code.

### 🧾 **Payment References**
- **Belgian OGM/VCS:** Generates and validates the `+++XXX/XXXX/XXXXX+++` format with automatic check digits.
- **ISO 11649 (RF):** Generates and validates international `RF` creditor references.

### 🏢 **Business Numbers**
- **Enterprise Numbers:** Validates Belgian KBO/BCE (Mod97) & French SIRET/SIREN (Luhn).
- **VAT Numbers:** Validates formatting and check digits for EU-27 countries.

---

## 📦 Installation

Install via the NuGet Package Manager:

```bash
dotnet add package Finova
````

Or via the Package Manager Console:

```powershell
Install-Package Finova
```

-----

## 📖 Quick Start

### 1\. Validate an IBAN

```csharp
using Finova.Belgium.Validators;

// Validates format and checksum (Does NOT check if account exists)
bool isValid = BelgianBankAccountValidator.ValidateBelgianIban("BE68539007547034");

if (isValid) 
{
    Console.WriteLine("Structure is valid");
}
```

### 2\. Validate a Payment Card

```csharp
using Finova.PaymentCards;

// Validates checksum (Luhn) and detects brand
var result = PaymentCardValidator.Validate("4532123456789012");

if (result.IsValid)
{
    Console.WriteLine($"Valid {result.Brand} Card"); // Output: Valid Visa Card
}
```

### 3\. Generate a Payment Reference

```csharp
using Finova.Belgium.Services;
using Finova.Core.Models;

var service = new BelgianPaymentReferenceService();

// Generate Belgian OGM (+++000/0012/34569+++)
string ogm = service.Generate("123456", PaymentReferenceFormat.Domestic);

// Generate ISO RF (RF89INVOICE2024)
string isoRef = service.Generate("INVOICE2024", PaymentReferenceFormat.IsoRf);
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

## 🔄 v1.2.0 — European Unification *(In Progress)*
- **Finova.Europe:** Unified wrapper package for all SEPA countries  
- **Smart Routing:** Auto-detect country rules via `EuropeValidator`  
- **Extensions:** FluentValidation integration package  

---

## 📋 v1.3.0 — Corporate Identity *(Planned)*
- **VAT Numbers:** EU VAT checksums (VIES offline syntax)  
- **Enterprise Numbers:** French SIRET/SIREN, Belgian KBO/BCE  
- **National IDs:** Netherlands KVK, Spain NIF/CIF  

---

## 🔮 v1.4.0 — Modern Payment Strings *(Future)*
- **EPC QR Code:** Payload builder for SEPA Credit Transfer  
- **Swiss QR:** Bill string parsing logic  

---

## 🔮 v1.5.0 — Global Routing *(Future)*
- **USA:** ABA routing number checksums  
- **Canada:** Transit number validation  
- **Australia:** BSB number validation  

---

## 🔭 Horizon *(Undetermined)*
- WASM support for Blazor  
- AI-assisted anomaly detection  

-----

## 🤝 Contributing

We welcome contributions\! Please see [CONTRIBUTING.md](https://www.google.com/search?q=CONTRIBUTING.md) for details.

## 📄 License

This project is licensed under the **MIT License**.

-----

**Made with ❤️ for the .NET Community**

[GitHub](https://github.com/fdivrusa/Finova) • [Issues](https://github.com/fdivrusa/Finova/issues)