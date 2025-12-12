# Finova Architecture

## Project Structure

Finova follows a **Monolithic Library** structure with separate core and extension projects.

```
src/
├── Finova/                            (Main library)
│   ├── Finova.csproj
│   ├── Countries/                     (Country-specific implementations)
│   │   ├── Europe/
│   │   │   ├── Belgium/
│   │   │   ├── France/
│   │   │   └── ...
│   ├── Services/
│   └── Validators/
├── Finova.Core/                       (Shared utilities & Interfaces)
│   └── Finova.Core.csproj
└── Finova.Extensions.FluentValidation/ (FluentValidation integration)
    └── Finova.Extensions.FluentValidation.csproj
```

### 1. Finova (Main Library)
- Contains the implementation for all supported countries.
- Organized by Region -> Country.
- References `Finova.Core`.

### 2. Finova.Core
- Contains shared interfaces (`IVatValidator`, `IIbanValidator`, etc.).
- Contains common utilities (Modulo97, Luhn, etc.).
- No external dependencies.

### 3. Finova.Extensions.FluentValidation
- Provides extension methods for FluentValidation (`MustBeValidIban`, etc.).
- Depends on `Finova` and `FluentValidation`.

## 📦 NuGet Package

The `Finova` NuGet package is a single package that includes:
- `Finova.dll`
- `Finova.Core.dll`
- `Finova.Extensions.FluentValidation.dll`

This ensures users get all functionality with a single installation.

## 🚀 Adding New Countries

To add a new country (e.g., Italy):

1.  **Create Folder Structure:**
    `src/Finova/Countries/Europe/Italy/`

2.  **Implement Validators:**
    - `Validators/ItalyIbanValidator.cs`
    - `Validators/ItalyVatValidator.cs`

3.  **Register (Optional):**
    - If using DI, ensure the new validators are registered in `Finova.Extensions.DependencyInjection`.

4.  **Test:**
    - Add unit tests in `tests/Finova.Tests/Countries/Europe/Italy/`.

## ✅ Testing

Run all tests:
```bash
dotnet test
```
