using Finova.Core.Common;

namespace Finova.Core.Identifiers;

/// <summary>
/// Interface for BBAN (Basic Bank Account Number) validators.
/// BBAN is the country-specific bank account format (e.g., Sort Code + Account Number in UK).
/// </summary>
public interface IBbanValidator : IValidator<string>
{
    /// <summary>
    /// Gets the country code (ISO 3166-1 alpha-2) for this validator.
    /// </summary>
    string CountryCode { get; }
}
