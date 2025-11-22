# Finova

<div align="center">

**Innovative financial toolkit for .NET**

*IBAN validation · Payment references · VAT validation · PEPPOL · UBL · SEPA*

[![NuGet](https://img.shields.io/nuget/v/Finova.svg?label=NuGet)](https://www.nuget.org/packages/Finova/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Finova.svg?label=Downloads)](https://www.nuget.org/packages/Finova/)
[![GitHub Package](https://img.shields.io/badge/GitHub-Package-blue?logo=github)](https://github.com/fdivrusa/Finova/packages)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

</div>

---

## 📊 Build Status

| Branch | CI | CD | Coverage |
|--------|----|----|----------|
| **master** | [![CI - master](https://github.com/fdivrusa/Finova/actions/workflows/ci.yml/badge.svg?branch=master)](https://github.com/fdivrusa/Finova/actions/workflows/ci.yml?query=branch%3Amaster) | [![CD](https://github.com/fdivrusa/Finova/actions/workflows/cd.yml/badge.svg)](https://github.com/fdivrusa/Finova/actions/workflows/cd.yml) | [![codecov](https://codecov.io/gh/fdivrusa/Finova/branch/master/graph/badge.svg)](https://codecov.io/gh/fdivrusa/Finova/branch/master) |
| **develop** | [![CI - develop](https://github.com/fdivrusa/Finova/actions/workflows/ci.yml/badge.svg?branch=develop)](https://github.com/fdivrusa/Finova/actions/workflows/ci.yml?query=branch%3Adevelop) | [![CD - develop](https://github.com/fdivrusa/Finova/actions/workflows/cd.yml/badge.svg?branch=develop)](https://github.com/fdivrusa/Finova/actions/workflows/cd.yml?query=branch%3Adevelop) | [![codecov](https://codecov.io/gh/fdivrusa/Finova/branch/develop/graph/badge.svg)](https://codecov.io/gh/fdivrusa/Finova/branch/develop) |

---

## 🌟 About Finova

**Finova** is a comprehensive **offline** financial validation library for .NET, designed for applications requiring fast, local validation of financial data. Built with European and international standards in mind, Finova provides production-ready tools for banking, tax, and invoicing operations.

### ⚡ Offline Validation Only

> **Important:** Finova performs **100% offline validation** based on format and checksum algorithms. It does **NOT** contact external services, APIs, or partners.

**What Finova Does (Offline):**
- ✅ Validates IBAN format and checksum (modulo 97)
- ✅ Generates payment references (OGM/VCS, ISO 11649)
- ✅ Validates KBO/BCE and VAT numbers (format + checksum)
- ✅ Extracts bank codes from IBAN structure

**What Finova Does NOT Do:**
- ❌ Does NOT verify if bank codes actually exist
- ❌ Does NOT verify if accounts actually exist
- ❌ Does NOT contact external APIs or services
- ❌ Does NOT require internet connection

### Why Finova?

- ✅ **100% Offline** - No external API calls, no internet required
- ✅ **Lightning Fast** - Instant validation (0-1ms per operation)
- ✅ **Production-Ready** - Battle-tested with 456+ unit tests and >95% code coverage
- ✅ **Standards-Compliant** - ISO 13616 (IBAN), ISO 11649 (RF), ISO 7064 (MOD 97)
- ✅ **International** - Multi-country support (Belgium, Netherlands, Luxembourg)
- ✅ **Modern** - Built for .NET 10.0+ with dependency injection support
- ✅ **Open Source** - MIT licensed, community-driven development
- ✅ **Zero Dependencies** - No external packages required

---

## 🚀 Features

### 💳 **IBAN Validation (Offline)** *(Available Now)*

Finova provides fast, offline IBAN validation for Benelux countries:

- **Belgian IBANs (BE)**
  - ✅ Format validation (16 characters: BE + 2 check + 12 digits)
  - ✅ Modulo 97 checksum validation (ISO 7064)
  - ✅ Bank code extraction (3 digits)
  - ✅ IBAN formatting with spaces
  - ❌ Does NOT verify if bank code exists
  
- **Dutch IBANs (NL)**
  - ✅ Format validation (18 characters: NL + 2 check + 4 letters + 10 digits)
  - ✅ Bank code format validation (4 letters only)
  - ✅ Account number format validation (10 digits only)
  - ✅ Modulo 97 checksum validation
  - ✅ Bank code extraction
  - ❌ Does NOT verify if bank code exists (e.g., ABNA, INGB, RABO)
  
- **Luxembourg IBANs (LU)**
  - ✅ Format validation (20 characters: LU + 2 check + 3 digits + 13 digits)
  - ✅ Bank code format validation (3 digits only)
  - ✅ Account number format validation (13 digits only)
  - ✅ Modulo 97 checksum validation
  - ✅ Bank code extraction
  - ❌ Does NOT verify if bank code exists

- **Generic IBAN (Any Country)**
  - ✅ Universal IBAN validation (ISO 13616)
  - ✅ Country code extraction
  - ✅ Check digits extraction
  - ✅ IBAN normalization and formatting

### 💳 **Belgian Banking & Payments** *(Available Now)*

Finova provides comprehensive Belgian banking support with production-ready implementations:

- **Structured Payment References (OGM/VCS)**
  - ✅ Belgian domestic format: `+++XXX/XXXX/XXXXX+++`
  - ✅ Automatic modulo 97 check digit calculation
  - ✅ Format validation and normalization
  - ✅ Up to 10-digit reference data support
  
- **International Payment References**
  - ✅ ISO 11649 (RF) creditor references
  - ✅ Format: `RFxx` + reference body
  - ✅ Automatic check digit calculation (modulo 97)
  - ✅ Full validation with checksum verification
  - ✅ Display format support (with spaces)

- **Belgian Business Numbers (Offline)**
  - ✅ KBO/BCE enterprise number validation (format + modulo 97)
  - ✅ VAT number validation (format + checksum)
  - ✅ Formatting with dots: `0403.170.701` or `BE0403.170.701`
  - ✅ Normalization (remove spaces, dots, BE prefix)
  - ❌ Does NOT verify if enterprise/VAT actually exists

- **Core Financial Utilities**
  - ✅ Modulo 97 calculations (ISO 7064)
  - ✅ Arbitrary-length numeric string support
  - ✅ IBAN/payment reference checksum validation
  - ✅ Type-safe payment reference format enum

- **Dependency Injection Support**
  - ✅ ASP.NET Core integration via `AddBelgianPaymentReference()`
  - ✅ Interface-based design (`IPaymentReferenceGenerator`, `IBankAccountValidator`)
  - ✅ Easy to extend for custom implementations
  - ✅ Singleton service registration

### 🏗️ **Architecture & Design**

- **100% Offline** - All validation done locally, no external API calls
- **Modular Design** - Separation of core (Finova.Core) and regional features
- **Interface-Based** - `IPaymentReferenceGenerator`, `IBankAccountValidator` for extensibility
- **Standards-Compliant** - ISO 13616 (IBAN), ISO 11649 (RF), ISO 7064 (MOD 97)
- **Production-Ready** - 456+ unit tests, >95% code coverage
- **Type-Safe** - Strong typing with comprehensive enums and models

### � **What Requires External Services** *(Not in NuGet)*

The following features require real-time verification with external partners and are **NOT** included in the NuGet package:

- ❌ **Bank Code Existence Verification** - Checking if "ABNA" is a real Dutch bank
- ❌ **Account Existence Verification** - Checking if an IBAN actually exists
- ❌ **Bank Information Lookup** - Getting bank name, BIC, address
- ❌ **KBO/BCE Real-time Verification** - Checking if enterprise is registered
- ❌ **VAT VIES Verification** - EU VAT number cross-border validation
- ❌ **Third-party API Integration** - Any external validation services

> **Note:** For real-time validation with external partners, consider building a separate API service that wraps Finova with external data sources.

### 🌍 **Future Offline Support** *(Roadmap)*

Additional offline validation features planned:

- **More IBAN Countries** - v1.1.0 (Q1 2026)
  - France (FR) format and checksum validation
  - Germany (DE) format and checksum validation
  - UK (GB) format and checksum validation
  - All offline, no external API calls
  
- **VAT Format Validation** - v1.2.0 (Q2 2026)
  - EU VAT number format validation (offline)
  - Format: country prefix + digits + check digit
  - Checksum validation where applicable
  - Note: Real-time VIES verification requires external API

- **Payment File Generation** - v1.3.0 (Q3 2026)
  - SEPA Credit Transfer (pain.001) XML generation
  - SEPA Direct Debit (pain.008) XML generation
  - All offline, no API calls

### 📄 **Features NOT Planned for NuGet** *(Require External Services)*

The following features require real-time integration with external partners and will **NOT** be added to this offline NuGet package:

- ❌ **VIES Real-time VAT Verification** - Requires EU VIES API
- ❌ **PEPPOL Integration** - Requires access point registration
- ❌ **UBL Document Exchange** - Requires document routing
- ❌ **Bank Registry Lookups** - Requires bank database access
- ❌ **Account Existence Checks** - Requires banking API access
- ❌ **Partner Integrations** - Any external service connectivity

> **Important:** Finova is designed as a **pure offline validation library** with zero dependencies and no external service integrations. For online features, you'll need to build a separate integration layer.

---

## 📦 Installation

### Stable Release

```bash
dotnet add package Finova
```

Or via Package Manager Console:
```powershell
Install-Package Finova
```

### Requirements

- **.NET 10.0** or higher
- **No external dependencies**
- **No internet connection required**
- **100% offline** - All validation runs locally

### Pre-release/Alpha

To install the latest alpha version with new features:

```bash
dotnet add package Finova --version *-alpha.*
```

Or via Package Manager Console:
```powershell
Install-Package Finova -PreRelease
```

> **Note:** Alpha versions are published from the `develop` branch (format: `1.0.0-alpha.{commits}+{sha}`). See [VERSIONING.md](VERSIONING.md) for details.

---

## 📖 Quick Start

> **⚡ Important: Offline Validation Only**
> 
> Finova performs **100% offline validation** based on mathematical algorithms (modulo 97, format checks).
> - ✅ Validates format and checksum
> - ❌ Does NOT contact external APIs or partners
> - ❌ Does NOT verify if banks/accounts actually exist
> - ❌ Does NOT require internet connection
> - ❌ Does NOT include any partner integrations

### IBAN Validation (Offline)

```csharp
using Finova.Belgium.Validators;
using Finova.Netherlands.Validators;
using Finova.Luxembourg.Validators;
using Finova.Core.Accounts;

// Belgian IBAN - format and checksum only
bool isValid = BelgianBankAccountValidator.ValidateBelgianIban("BE68539007547034");
// Returns: true (valid format + checksum)

// Extract bank code (structure only, doesn't verify bank exists)
string? bankCode = BelgianBankAccountValidator.GetBankCode("BE68539007547034");
// Returns: "539" (extracted from IBAN, existence NOT verified)

// Dutch IBAN - validates format, bank code structure (4 letters), checksum
bool isValid = DutchBankAccountValidator.ValidateDutchIban("NL91ABNA0417164300");
string? bankCode = DutchBankAccountValidator.GetBankCode("NL91ABNA0417164300");
// Returns: "ABNA" (extracted, does NOT verify if ABNA bank exists)

// Luxembourg IBAN - validates format, bank code structure (3 digits), checksum
bool isValid = LuxembourgBankAccountValidator.ValidateLuxembourgIban("LU280019400644750000");
string? bankCode = LuxembourgBankAccountValidator.GetBankCode("LU280019400644750000");
// Returns: "001" (extracted, does NOT verify if bank 001 exists)

// Generic IBAN - any country (format + checksum only)
bool isValid = IbanHelper.IsValidIban("FR1420041010050500013M02606");
string countryCode = IbanHelper.GetCountryCode("FR1420041010050500013M02606");
// Returns: "FR"
```

### Belgian Payment References

```csharp
using Finova.Belgium.Services;
using Finova.Core.Models;

// Create service instance
var service = new BelgianPaymentReferenceService();

// Generate Belgian OGM/VCS structured communication
string ogm = service.Generate("123456", PaymentReferenceFormat.Domestic);
// Output: +++000/0012/34569+++

// Generate ISO 11649 international reference
string isoRef = service.Generate("INVOICE2024", PaymentReferenceFormat.IsoRf);
// Output: RF89INVOICE2024

// Validate payment reference
bool isValid = service.IsValid("+++000/0012/34569+++");
// Output: true
```

### Dependency Injection (ASP.NET Core)

```csharp
using Finova.Belgium.Extensions;

// In Program.cs
builder.Services.AddBelgianPaymentReference();

// In your service
public class InvoiceService
{
    private readonly IPaymentReferenceGenerator _paymentRefGenerator;

    public InvoiceService(IPaymentReferenceGenerator paymentRefGenerator)
    {
        _paymentRefGenerator = paymentRefGenerator;
    }

    public string CreateInvoice(int invoiceNumber)
    {
        var paymentRef = _paymentRefGenerator.Generate(
            invoiceNumber.ToString(), 
            PaymentReferenceFormat.Domestic
        );
        
        return paymentRef; // +++XXX/XXXX/XXXXX+++
    }
}
```

### ISO 11649 References

```csharp
using Finova.Core.Internals;

// Generate ISO 11649 reference
string reference = IsoReferenceHelper.Generate("CUSTOMER12345");
// Output: RF23CUSTOMER12345

// Validate ISO 11649 reference
bool isValid = IsoReferenceValidator.IsValid("RF23CUSTOMER12345");
// Output: true

// Works with spaces (display format)
bool isValid2 = IsoReferenceValidator.IsValid("RF23 CUSTOMER 12345");
// Output: true
```

### Modulo 97 Calculations

```csharp
using Finova.Core.Internals;

// Calculate modulo 97 of a numeric string
int result = Modulo97Helper.Calculate("1234567890");
// Output: 37

// Works with very large numbers
int result2 = Modulo97Helper.Calculate("123456789012345678901234567890");
// Returns correct modulo 97 result
```

---

## � Use Cases

<table>
<tr>
<td width="50%">

### **Financial Services**
- Payment processing
- IBAN verification
- SEPA file generation
- Bank account validation

</td>
<td width="50%">

### **E-Commerce**
- Invoice payment references
- Multi-country payments
- Payment validation
- Order processing

</td>
</tr>
<tr>
<td width="50%">

### **Accounting & ERP**
- VAT validation
- Tax identifiers
- Enterprise numbers
- Multi-currency invoicing

</td>
<td width="50%">

### **E-Invoicing**
- PEPPOL compliance
- UBL generation
- EN 16931 compliance
- Digital invoice exchange

</td>
</tr>
</table>

---

## 🏗️ Architecture

### Project Structure

### Project Structure

```
Finova (NuGet Package)
├── Finova.Core              → Shared utilities, interfaces, algorithms
└── Finova.Belgium           → Belgian banking features (payment references)

Future country modules (extensible architecture):
├── Finova.France            → French banking features (v1.1+)
├── Finova.Italy             → Italian banking features (v1.1+)
├── Finova.Netherlands       → Dutch banking features (v1.1+)
└── ...                      → Additional countries as needed
```

**Architecture Highlights:**
- **Single Package**: All country implementations bundled in one `Finova` NuGet package
- **Modular Development**: Each country is a separate project for maintainability
- **Clean Namespaces**: `Finova.Belgium`, `Finova.France`, etc. for easy identification
- **Extensible**: Add new countries without breaking existing code
- **Zero Dependencies**: No need to install multiple packages

### Core Library (`Finova.Core`)

Provides foundational utilities:
- `IPaymentReferenceGenerator` - Payment reference interface
- `IBankAccountValidator` - IBAN validation interface *(coming v1.1)*
- `Modulo97Helper` - ISO 7064 modulo 97 calculations
- `IsoReferenceHelper` - ISO 11649 reference generation
- `IsoReferenceValidator` - ISO 11649 validation
- `PaymentReferenceFormat` - Format types enum

### Belgian Implementation (`Finova.Belgium`)

Belgian-specific features:
- `BelgianPaymentReferenceService` - Implements `IPaymentReferenceGenerator`
  - OGM/VCS format (+++XXX/XXXX/XXXXX+++)
  - ISO 11649 format support
  - Complete validation logic
- `ServiceCollectionExtensions` - DI registration helpers (`AddBelgianPaymentReference()`)

### Future Implementations

When you add a new country (e.g., France):
1. Create `src/Finova.France/` project
2. Add `<ProjectReference>` in `src/Finova/Finova.csproj`
3. Implement country-specific features
4. Automatically included in the `Finova` NuGet package!

### Extensibility

**Adding Custom Country Implementations:**

```csharp
// 1. Create your country-specific service
public class FrenchPaymentService : IPaymentReferenceGenerator
{
    public string CountryCode => "FR";
    
    public string Generate(string rawReference, PaymentReferenceFormat format)
    {
        // Your custom implementation for French payment references
        return /* ... */;
    }
    
    public bool IsValid(string reference)
    {
        // Your validation logic
        return /* ... */;
    }
}

// 2. Register with DI (or use directly)
services.AddSingleton<IPaymentReferenceGenerator, FrenchPaymentService>();

// 3. Or access directly
var frenchService = new FrenchPaymentService();
```

**Multi-Country Support in One Application:**

```csharp
// Register multiple country implementations
services.AddBelgianPaymentReference();  // Belgium
// Future: services.AddFrenchPaymentReference();   // France
// Future: services.AddItalianPaymentReference();  // Italy

// Resolve all registered implementations
public class MultiCountryPaymentService
{
    private readonly IEnumerable<IPaymentReferenceGenerator> _generators;
    
    public MultiCountryPaymentService(IEnumerable<IPaymentReferenceGenerator> generators)
    {
        _generators = generators;
    }
    
    public string GenerateForCountry(string countryCode, string reference)
    {
        var generator = _generators.FirstOrDefault(g => g.CountryCode == countryCode);
        return generator?.Generate(reference, PaymentReferenceFormat.Domestic) 
               ?? throw new NotSupportedException($"Country {countryCode} not supported");
    }
}
```

---

## 🧪 Quality & Testing

- ✅ **106 Unit Tests** - Comprehensive test coverage
- ✅ **>95% Code Coverage** - High quality assurance
- ✅ **CI/CD Pipeline** - Automated build, test, and deployment
- ✅ **Code Quality** - Linting and formatting checks
- ✅ **Performance** - Benchmarked for production use

Run tests:
```bash
dotnet test
```

View coverage reports:
- [Master Branch Coverage](https://codecov.io/gh/fdivrusa/Finova/tree/master)
- [Develop Branch Coverage](https://codecov.io/gh/fdivrusa/Finova/tree/develop)

- [Develop Branch Coverage](https://codecov.io/gh/fdivrusa/Finova/tree/develop)

---

## 🚀 CI/CD Pipeline

### Continuous Integration (CI)

Runs automatically on every push or pull request:

| Branch | Status | Trigger | Actions |
|--------|--------|---------|---------|
| **master** | [![CI - master](https://github.com/fdivrusa/Finova/actions/workflows/ci.yml/badge.svg?branch=master)](https://github.com/fdivrusa/Finova/actions/workflows/ci.yml?query=branch%3Amaster) | Push or PR | Build, test, coverage, linting |
| **develop** | [![CI - develop](https://github.com/fdivrusa/Finova/actions/workflows/ci.yml/badge.svg?branch=develop)](https://github.com/fdivrusa/Finova/actions/workflows/ci.yml?query=branch%3Adevelop) | Push or PR | Build, test, coverage, linting |

### Continuous Deployment (CD)

Manual workflow dispatch with automatic branch-based versioning:

- **Trigger**: ⚙️ Manual dispatch or 🏷️ GitHub release
- **Destinations**: NuGet.org + GitHub Packages
- **Status**: [![CD](https://github.com/fdivrusa/Finova/actions/workflows/cd.yml/badge.svg)](https://github.com/fdivrusa/Finova/actions/workflows/cd.yml)

### Versioning Strategy

| Branch | Format | Example | Description |
|--------|--------|---------|-------------|
| **master** | `{base}.{commits}` | `1.0.0.123` | Stable production releases |
| **develop** | `{base}-alpha.{commits}+{sha}` | `1.0.0-alpha.42+a1b2c3d` | Alpha pre-releases |

**To publish:**
1. Go to [Actions → CD Workflow](https://github.com/fdivrusa/Finova/actions/workflows/cd.yml)
2. Click "Run workflow"
3. Select branch (`master` or `develop`)
4. Version is automatically determined

See [VERSIONING.md](VERSIONING.md) for complete details.

---

## 🤝 Contributing

We welcome contributions! Here's how you can help:

### Priority Areas
1. 🌍 **Country Implementations** - Add IBAN, VAT, payment formats for your country
2. � **PEPPOL & UBL** - Help build e-invoicing support
3. 🧪 **Testing** - Add edge cases and scenarios
4. 📖 **Documentation** - Examples, guides, translations
5. ⚡ **Performance** - Benchmarking and optimization

### Development Setup

```bash
# Clone repository
git clone https://github.com/fdivrusa/Finova.git
cd Finova

# Restore dependencies
dotnet restore

# Build solution
dotnet build

# Run tests
dotnet test
```

### Branch Strategy
- `master` - Stable releases (production-ready)
- `develop` - Development branch (alpha releases)
- Feature branches - Create from `develop`, merge back to `develop`

---

## 📋 Requirements

- **.NET 10.0** or higher
- **Microsoft.Extensions.DependencyInjection 10.0.0+** (for DI support)

---

## 📄 License

This project is licensed under the **MIT License** - see the [LICENSE](LICENSE) file for details.

---

## �🗺️ Roadmap

### ✅ v1.0.0 - Foundation (Released)
- Belgian payment references (OGM/VCS)
- ISO 11649 international references
- Comprehensive testing and CI/CD

### 🔄 v1.1.0 - European Banking (Q1 2026)
- [ ] IBAN validation (BE, NL, FR, DE, LU, UK)
- [ ] BIC/SWIFT code validation
- [ ] Bank code to BIC mapping
- [ ] Legacy account number conversion

### 📋 v1.2.0 - Tax & Business (Q2 2026)
- [ ] VAT number validation (EU-27)
- [ ] VIES real-time integration
- [ ] Enterprise number validation (KBO/BCE)
- [ ] Tax identifier validation

### 📋 v1.3.0 - SEPA Payments (Q3 2026)
- [ ] SEPA Credit Transfer (pain.001)
- [ ] SEPA Direct Debit (pain.008)
- [ ] XML file generation
- [ ] Batch payment support

### 📋 v1.4.0 - PEPPOL Foundation (Q4 2026)
- [ ] PEPPOL participant ID validation
- [ ] Document type identifiers
- [ ] PEPPOL BIS 3.0 support
- [ ] Endpoint validation

### 📋 v2.0.0 - E-Invoicing Suite (Q1 2027)
- [ ] UBL 2.1 invoice generation
- [ ] EN 16931 compliance
- [ ] Credit notes and debit notes
- [ ] Cross Industry Invoice (CII)

### 📋 v2.1.0+ - Country Expansion (Q2+ 2027)
- [ ] Country-specific e-invoicing (DE, FR, IT, ES)
- [ ] XRechnung, Factur-X, FatturaPA
- [ ] Additional SEPA countries
- [ ] Global expansion (US, AU, SG, etc.)

**See [ROADMAP.md](ROADMAP.md) for detailed feature breakdown.**

---

## 🔧 Supported Standards

### Current
- **ISO 11649** - International payment references (RF creditor reference)
- **ISO 7064** - Modulo 97 checksum algorithm
- **Belgian OGM/VCS** - Structured communication format

### Coming Soon
- **ISO 13616** - IBAN structure and validation (v1.1)
- **ISO 9362** - BIC/SWIFT codes (v1.1)
- **EN 16931** - European e-invoicing semantic model (v2.0)
- **PEPPOL BIS 3.0** - Business Interoperability Specifications (v1.4)
- **UBL 2.1** - Universal Business Language (v2.0)
- **ISO 20022** - SEPA payment messages (v1.3)

---

## 🌍 Country Support

### Current Support 🇧🇪
- **Belgium** - Payment references (OGM/VCS, ISO 11649)

### Coming v1.1-1.2
- 🇧🇪 **Belgium** - IBAN, VAT, enterprise numbers
- �🇱 **Netherlands** - IBAN, VAT
- 🇫🇷 **France** - IBAN, VAT
- 🇩� **Germany** - IBAN, VAT
- 🇱🇺 **Luxembourg** - IBAN, VAT
- 🇬🇧 **United Kingdom** - IBAN

### Future Plans
- 🇮🇹 Italy, 🇪🇸 Spain, 🇦🇹 Austria, 🇸🇪 Sweden, 🇵🇹 Portugal
- More EU countries and international expansion

**Want to add your country?** See [CONTRIBUTING.md](CONTRIBUTING.md)!

---

## � API Reference

### Core Interfaces

#### IPaymentReferenceGenerator

```csharp
public interface IPaymentReferenceGenerator
{
    string CountryCode { get; }
    string Generate(string rawReference, PaymentReferenceFormat format = PaymentReferenceFormat.Domestic);
    bool IsValid(string communication);
}
```

### Belgian Implementation

#### BelgianPaymentReferenceService

```csharp
public class BelgianPaymentReferenceService : IPaymentReferenceGenerator
{
    public string CountryCode => "BE";
    
    // Generate payment reference
    public string Generate(string rawReference, 
        PaymentReferenceFormat format = PaymentReferenceFormat.Domestic);
    
    // Validate payment reference
    public bool IsValid(string communication);
}
```

**Formats:**
- `PaymentReferenceFormat.Domestic` → `+++XXX/XXXX/XXXXX+++` (Belgian OGM)
- `PaymentReferenceFormat.IsoRf` → `RFxxYYYY...` (ISO 11649)

### Core Utilities

#### IsoReferenceHelper

```csharp
public static class IsoReferenceHelper
{
    // Generate ISO 11649 reference with RF prefix
    public static string Generate(string rawReference);
}
```

**Format**: `RFxx` (RF + 2 check digits) + reference body  
**Example**: `IsoReferenceHelper.Generate("INVOICE2024")` → `RF89INVOICE2024`

#### IsoReferenceValidator

```csharp
public static class IsoReferenceValidator
{
    // Validate ISO 11649 reference format and checksum
    public static bool IsValid(string reference);
}
```

**Features**:
- Validates RF prefix
- Verifies modulo 97 checksum
- Accepts spaces (display format)

#### Modulo97Helper

```csharp
public static class Modulo97Helper
{
    // Calculate modulo 97 of numeric string (ISO 7064)
    public static int Calculate(string numericString);
}
```

**Features**:
- Handles arbitrarily large numbers
- ISO 7064 compliant
- Used for IBAN, ISO 11649, OGM checksums

### Extensions

#### ServiceCollectionExtensions

```csharp
public static class ServiceCollectionExtensions
{
    // Register Belgian banking services with DI
    public static IServiceCollection AddBelgianPaymentReference(
        this IServiceCollection services);
}
```

**Registers**:
- `IPaymentReferenceGenerator` → `BelgianPaymentReferenceService`

---

## 📚 Documentation

- [Getting Started Guide](docs/getting-started.md) *(coming soon)*
- [API Reference](#-api-reference)
- [PEPPOL Guide](docs/peppol-guide.md) *(coming soon)*
- [Contributing Guidelines](CONTRIBUTING.md) *(coming soon)*
- [Versioning Strategy](VERSIONING.md)
- [Package Metadata](PACKAGE_METADATA.md)
- [Detailed Roadmap](ROADMAP.md) *(coming soon)*

---

## 🙏 Acknowledgments

- **ISO 11649** - International payment reference standard
- **ISO 7064** - Modulo 97 checksum algorithm
- **Belgian Banking Standards** - OGM/VCS format specification
- **European Payments Council** - SEPA standards
- **PEPPOL** - Pan-European Public Procurement On-Line
- **.NET Foundation** - For the amazing .NET platform

---

## 💬 Community & Support

### Get Help
- 📖 [Documentation](#-documentation)
- 💬 [GitHub Discussions](https://github.com/fdivrusa/Finova/discussions)
- 🐛 [Issue Tracker](https://github.com/fdivrusa/Finova/issues)
- 📧 [Contact](mailto:your.email@example.com)

### Stay Updated
- ⭐ Star this repository
- 👀 Watch for releases
- 📢 Follow development on GitHub
- 📝 Read the [CHANGELOG](CHANGELOG.md) *(coming soon)*

---

<div align="center">

**Made with ❤️ for the European financial community**

[Website](https://finova.dev) • [Documentation](https://docs.finova.dev) • [NuGet](https://nuget.org/packages/Finova) • [GitHub](https://github.com/fdivrusa/Finova)

*Finova - Innovative financial operations for .NET*

</div>
