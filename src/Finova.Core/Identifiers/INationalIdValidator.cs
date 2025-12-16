using Finova.Core.Common;

namespace Finova.Core.Identifiers;

/// <summary>
/// Interface for National Identification Number validators (e.g., SSN, SIN, CPF, Aadhaar).
/// </summary>
public interface INationalIdValidator : IValidator<string>
{
    /// <summary>
    /// Gets the country code (ISO 3166-1 alpha-2) for this validator.
    /// </summary>
    string CountryCode { get; }
}
