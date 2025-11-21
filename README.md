# BankingHelper

[![CI](https://github.com/yourusername/BankingHelper/actions/workflows/ci.yml/badge.svg)](https://github.com/yourusername/BankingHelper/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/BankingHelper.Core.svg)](https://www.nuget.org/packages/BankingHelper.Core/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A comprehensive .NET library for banking operations including IBAN validation, payment reference generation, and country-specific banking utilities. Built with a modular architecture to support multiple countries and banking standards.

## 🌟 Features

- **Payment Reference Generation**
  - ISO 11649 (RF) international payment references
  - Country-specific formats (currently supports Belgium OGM/VCS)
  - Automatic check digit calculation
  - Format validation

- **Modular Architecture**
  - Core library with shared utilities
  - Country-specific implementations as separate packages
  - Dependency injection support
  - Extensible interfaces for custom implementations

- **Belgian Banking Support**
  - OGM/VCS structured communication (+++XXX/XXXX/XXXXX+++)
  - ISO 11649 format support
  - Complete validation logic
  - Easy integration with ASP.NET Core

## 📦 Packages

| Package | Description | NuGet |
|---------|-------------|-------|
| `BankingHelper.Core` | Core interfaces and utilities | [![NuGet](https://img.shields.io/nuget/v/BankingHelper.Core.svg)](https://www.nuget.org/packages/BankingHelper.Core/) |
| `BankingHelper.Belgium` | Belgian banking implementation | [![NuGet](https://img.shields.io/nuget/v/BankingHelper.Belgium.svg)](https://www.nuget.org/packages/BankingHelper.Belgium/) |

## 🚀 Installation

Install the packages via NuGet Package Manager or .NET CLI:

### Core Package
```bash
dotnet add package BankingHelper.Core
```

### Belgian Banking Support
```bash
dotnet add package BankingHelper.Belgium
```

## 📖 Usage

### Basic Usage - Belgian Payment References

```csharp
using BankingHelper.Belgium.Services;
using BankingHelper.Core.Models;

// Create an instance of the Belgian payment service
var service = new BelgianPaymentService();

// Generate a Belgian OGM/VCS structured communication
string ogm = service.Generate("123456", PaymentReferenceFormat.Domestic);
// Output: +++000/0012/34569+++

// Generate an ISO 11649 international reference
string isoRef = service.Generate("INVOICE2024", PaymentReferenceFormat.IsoRf);
// Output: RF89INVOICE2024

// Validate a payment reference
bool isValid = service.IsValid("+++000/0012/34569+++");
// Output: true
```

### Dependency Injection (ASP.NET Core)

```csharp
using BankingHelper.Belgium.Extensions;

// In Program.cs or Startup.cs
builder.Services.AddBelgianBanking();

// In your controller or service
public class InvoiceService
{
    private readonly IPaymentReferenceGenerator _paymentRefGenerator;

    public InvoiceService(IPaymentReferenceGenerator paymentRefGenerator)
    {
        _paymentRefGenerator = paymentRefGenerator;
    }

    public string CreateInvoice(int invoiceNumber)
    {
        // Generate payment reference
        var paymentRef = _paymentRefGenerator.Generate(
            invoiceNumber.ToString(), 
            PaymentReferenceFormat.Domestic
        );
        
        return paymentRef;
    }
}
```

### Working with ISO 11649 References

```csharp
using BankingHelper.Core.Internals;

// Generate an ISO 11649 reference
string reference = IsoReferenceHelper.Generate("CUSTOMER12345");
// Output: RF23CUSTOMER12345

// Validate an ISO 11649 reference
bool isValid = IsoReferenceValidator.IsValid("RF23CUSTOMER12345");
// Output: true

// Works with spaces (common in display format)
bool isValid2 = IsoReferenceValidator.IsValid("RF23 CUSTOMER 12345");
// Output: true
```

### Modulo 97 Calculations

```csharp
using BankingHelper.Core.Internals;

// Calculate modulo 97 of a numeric string
int result = Modulo97Helper.Calculate("1234567890");
// Output: 37

// Works with very large numbers
int result2 = Modulo97Helper.Calculate("123456789012345678901234567890");
// Returns correct modulo 97 result
```

## 🏗️ Architecture

### Core Library (`BankingHelper.Core`)

The core library provides:
- `IPaymentReferenceGenerator` - Interface for payment reference generation
- `IBankAccountValidator` - Interface for IBAN validation (future feature)
- `Modulo97Helper` - ISO 7064 modulo 97 calculations
- `IsoReferenceHelper` - ISO 11649 reference generation
- `IsoReferenceValidator` - ISO 11649 reference validation
- `PaymentReferenceFormat` - Enum for different format types

### Belgian Implementation (`BankingHelper.Belgium`)

The Belgian implementation includes:
- `BelgianPaymentService` - Implements `IPaymentReferenceGenerator`
  - OGM/VCS format (+++XXX/XXXX/XXXXX+++)
  - ISO 11649 format support
  - Complete validation logic
- `ServiceCollectionExtensions` - DI registration helpers

## 🧪 Testing

The project includes comprehensive unit tests covering:
- All payment reference generation scenarios
- Edge cases and error handling
- Format validation
- Integration tests
- Dependency injection setup

Run tests with:
```bash
dotnet test
```

## 🔧 Supported Formats

### Belgian OGM/VCS (Structured Communication)

Format: `+++XXX/XXXX/XXXXX+++`
- 12 digits total (10 data + 2 check digits)
- Modulo 97 checksum
- Common in Belgian banking for invoice payments

### ISO 11649 (RF Creditor Reference)

Format: `RFxxYYYY...`
- Starts with "RF" prefix
- 2 check digits (calculated using modulo 97)
- Variable length reference body (up to 25 characters)
- International standard for payment references

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request. Here are some ways you can contribute:

- Add support for other countries (IBAN validators, payment references)
- Improve documentation
- Add more test cases
- Report bugs or suggest features

### Development Setup

1. Clone the repository
```bash
git clone https://github.com/yourusername/BankingHelper.git
```

2. Restore dependencies
```bash
dotnet restore
```

3. Build the solution
```bash
dotnet build
```

4. Run tests
```bash
dotnet test
```

## 📋 Requirements

- .NET 10.0 or higher
- For Belgium package: Microsoft.Extensions.DependencyInjection 10.0.0+

## 🗺️ Roadmap

- [ ] IBAN validation for multiple countries
- [ ] Additional country implementations (France, Netherlands, Germany, etc.)
- [ ] SEPA payment file generation
- [ ] BIC/SWIFT code validation
- [ ] Bank account number normalization

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🙏 Acknowledgments

- ISO 11649 Standard for RF Creditor Reference
- ISO 7064 for modulo 97 checksum algorithm
- Belgian banking standards for OGM/VCS format

## 📞 Support

If you encounter any issues or have questions:
- Open an issue on [GitHub](https://github.com/yourusername/BankingHelper/issues)
- Check existing documentation and tests for examples
- Review the API reference below

## 📚 API Reference

### IPaymentReferenceGenerator

```csharp
public interface IPaymentReferenceGenerator
{
    string CountryCode { get; }
    string Generate(string rawReference, PaymentReferenceFormat format = PaymentReferenceFormat.Domestic);
    bool IsValid(string communication);
}
```

### BelgianPaymentService

```csharp
public class BelgianPaymentService : IPaymentReferenceGenerator
{
    public string CountryCode => "BE";
    public string Generate(string rawReference, PaymentReferenceFormat format = PaymentReferenceFormat.Domestic);
    public bool IsValid(string communication);
}
```

### IsoReferenceHelper

```csharp
public static class IsoReferenceHelper
{
    public static string Generate(string rawReference);
}
```

### IsoReferenceValidator

```csharp
public static class IsoReferenceValidator
{
    public static bool IsValid(string reference);
}
```

### Modulo97Helper

```csharp
public static class Modulo97Helper
{
    public static int Calculate(string numericString);
}
```

---

Made with ❤️ for the .NET community
