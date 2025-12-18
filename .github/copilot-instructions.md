# Finova Project Instructions

## 1. Project Context & Architecture

### 1.1. Core Identity
- **What is Finova?**: A .NET Nuget package for **offline-first** financial validation (VAT, IBAN, BIC, Payment References).
- **Zero Dependencies**: The library must **never** make HTTP calls, database connections, or file system access at runtime.
- **Performance**: Use `ReadOnlySpan<char>` for parsing. Prefer static methods (e.g., `ValidateStatic`) for high-performance paths.

### 1.2. Structural Patterns
- **Monolithic Library**: `src/Finova` contains all country logic. `src/Finova.Core` contains shared interfaces.
- **Namespace Convention**:
  - `Finova.Core`: Interfaces (`IVatValidator`), Enums, Common Types.
  - `Finova.Countries.Europe.[CountryName]`: Country-specific logic (e.g., `Finova.Countries.Europe.Belgium`).
  - `Finova.Services`: High-level facades (`EuropeVatValidator`) and metadata.
- **Metadata-Driven**: Use `EuropeCountryIdentifierMetadata` to define capabilities. Avoid hardcoding country logic in random places.

## 2. Global Agent Rules

- **Persona**: Act as a **senior software engineer**. Produce clean, self-documenting, production-ready code.
- **Scope**: This is a library. Code must be robust, thread-safe, and exception-free for validation logic.
- **Proactive Refactoring**: If you see duplication, extract it into a helper *immediately*. Do not wait for instructions.

## 3. Coding Standards

### 3.1. Syntax & Formatting
- **Control Flow**: All control flow statements (`if`, `for`, `foreach`, `while`) **must** use curly braces `{ }`, even for single lines.
  ```csharp
  // ✅ Correct
  if (isValid)
  {
      return;
  }

  // ❌ Incorrect
  if (isValid) return;
  ```
- **Naming**: PascalCase for classes/methods. camelCase for variables. UPPER_SNAKE_CASE for constants.

### 3.2. Error Handling
- **No Exceptions for Validation**: Validation failures must return a `ValidationResult` object.
- **Exceptions**: Only throw for developer errors (e.g., `ArgumentNullException` in public API entry points).

### 3.3. Documentation
- **XML Documentation**: **Mandatory** for ALL public and internal members (Classes, Methods, Enums).
  - Must include `<summary>`, `<param>`, and `<returns>`.
- **Inline Comments**: **Forbidden** for obvious logic. Allowed only for complex business rules or specific algorithm explanations (e.g., "Mod97 algorithm step 3").
- **Markdown Files**: Update `README.md`, `ARCHITECTURE.md`, etc., if your changes affect the project structure or usage.

### 3.4. Localization & Resources
- **No Hardcoded Strings**: Validation error messages must NOT be hardcoded strings.
- **Resource Files**: Use `ValidationMessages` (backed by `.resx`) for all user-facing strings.

## 4. Testing Strategy

### 4.1. Mandatory Unit Tests
- **Rule**: Every new class or method must have corresponding Unit Tests generated **immediately**.
- **Coverage**:
  - Happy paths (Valid inputs).
  - Edge cases (Boundary values).
  - Invalid inputs (Length, Checksum, Characters).
  - Null/Empty inputs.
- **Location**: `tests/Finova.Tests/Countries/[Region]/[Country]/`.

## 5. Reuse & Abstraction

- **Generic Helpers**: Before writing a new validator, check `Finova.Core` for existing helpers (e.g., `Mod97Helper`, `LuhnAlgorithm`).
- **Duplication**: Rewriting the same logic in multiple places is a design flaw. Extract shared logic to `Finova.Core` or internal helpers.

## 6. Specific Implementation Details

- **Payment References**: Implement `IPaymentReferenceService`. Use naming `[Country]PaymentReferenceService`.
- **Sanitization**: Always sanitize input (trim, uppercase, remove separators) before validation using `VatSanitizer` or similar helpers.
