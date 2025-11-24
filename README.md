# Finova

<div align="center">

**The Offline Financial Validation Toolkit for .NET**

*IBAN · Payment References · KBO/VAT · Business Numbers*

[![NuGet Version](https://img.shields.io/nuget/v/Finova.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/Finova/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Finova.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/Finova/)
[![Build Status](https://github.com/fdivrusa/Finova/actions/workflows/ci.yml/badge.svg)](https://github.com/fdivrusa/Finova/actions/workflows/ci.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

**🇧🇪 Belgium · 🇳🇱 Netherlands · 🇱🇺 Luxembourg**

*100% Offline | Zero Dependencies | Lightning Fast*

</div>

---

## 🌟 About Finova

**Finova** is a comprehensive **offline** financial validation library for .NET. It allows you to validate financial data (IBANs, VAT numbers, Payment References) using official checksum algorithms (Mod97, ISO 7064) and regex patterns directly on your server.

### ⚡ Offline Validation Only

> **Important:** Finova performs **100% offline validation**. It does **NOT** contact external services, APIs, or banking networks.

**What Finova Does (Offline):**
- ✅ Validates IBAN format and checksum (ISO 7064 Mod 97)
- ✅ Generates and validates payment references (OGM/VCS, ISO 11649)
- ✅ Validates KBO/BCE and VAT numbers (Syntax + Checksum)
- ✅ Extracts bank codes from IBAN structure

**What Finova Does NOT Do:**
- ❌ Does NOT verify if an account/IBAN actually exists at the bank
- ❌ Does NOT perform real-time VIES VAT lookups
- ❌ Does NOT contact external APIs
- ❌ Does NOT require an internet connection

---

## 🚀 Features

### 💳 **IBAN Validation**
Fast, offline regex and checksum validation for Benelux and International formats.
- **Belgium (BE):** Format + Mod97 + Bank Code extraction.
- **Netherlands (NL):** Format + Mod97 + Bank Code extraction (4-letter codes).
- **Luxembourg (LU):** Format + Mod97 + Account number structure.
- **Generic (ISO 13616):** Supports parsing and validating checksums for all ISO-compliant countries.

### 🧾 **Payment References**
- **Belgian OGM/VCS:** Generates and validates the `+++XXX/XXXX/XXXXX+++` format with automatic check digits.
- **ISO 11649 (RF):** Generates and validates international `RF` creditor references.

### 🏢 **Business Numbers**
- **Enterprise Numbers (KBO/BCE):** Validates Belgian company numbers via Mod97.
- **VAT Numbers:** Validates formatting and check digits.

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

### 2\. Generate a Payment Reference

```csharp
using Finova.Belgium.Services;
using Finova.Core.Models;

var service = new BelgianPaymentReferenceService();

// Generate Belgian OGM (+++000/0012/34569+++)
string ogm = service.Generate("123456", PaymentReferenceFormat.Domestic);

// Generate ISO RF (RF89INVOICE2024)
string isoRef = service.Generate("INVOICE2024", PaymentReferenceFormat.IsoRf);
```

### 3\. Dependency Injection (ASP.NET Core)

```csharp
// Program.cs
builder.Services.AddBelgianPaymentReference();

// Service
public class InvoiceService(IPaymentReferenceGenerator generator)
{
    public string CreateRef(string id) => generator.Generate(id);
}
```

-----

## 🗺️ Roadmap

Finova is strictly offline. Future updates focus on **schema compliance**, **file generation**, and **mathematical validation**.

### ✅ v1.0.0 - Foundation (Released)

  - Belgian payment references (OGM/VCS)
  - ISO 11649 international references
  - Comprehensive testing and CI/CD

### 🔄 v1.1.0 - European Banking (Q1 2026)

  - IBAN validation rules (BE, NL, FR, DE, LU, UK)
  - BIC/SWIFT format validation (Regex)
  - Local Bank Code extraction logic
  - Legacy account conversion algorithms

### 📋 v1.2.0 - Tax & Business (Q2 2026)

  - VAT number syntax & checksums (EU-27)
  - **Offline** VIES syntax compliance checks
  - Enterprise number validation (KBO/BCE)
  - National Tax ID checksums

### 📋 v1.3.0 - SEPA File Standards (Q3 2026)

  - SEPA Credit Transfer (pain.001) **Schema Validation**
  - SEPA Direct Debit (pain.008) **Structure Checks**
  - Offline XML Builder (String Generation)
  - Batch file structure verification

### 📋 v1.4.0 - PEPPOL Compliance (Q4 2026)

  - Participant ID syntax validation
  - Document type identifier parsing
  - PEPPOL BIS 3.0 **XSD Verification**
  - Offline Scheme ID checks

### 📋 v2.0.0 - E-Invoicing Suite (Q1 2027)

  - UBL 2.1 invoice XML generation
  - EN 16931 structure compliance
  - Offline Credit & Debit note creation
  - Cross Industry Invoice (CII) support

### 📋 v2.1.0+ - Country Expansion (Q2+ 2027)

  - Local E-Invoicing Formats (XRechnung, Factur-X)
  - Expanded EU-27 Validation Rules
  - Global ID Formats (US, AU, SG)

-----

## 🤝 Contributing

We welcome contributions\! Please see [CONTRIBUTING.md](https://www.google.com/search?q=CONTRIBUTING.md) for details.

## 📄 License

This project is licensed under the **MIT License**.

-----
**Made with ❤️ for the .NET Community**

[GitHub](https://github.com/fdivrusa/Finova) • [Issues](https://github.com/fdivrusa/Finova/issues)