using Finova.Core.Common;

namespace Finova.Core.Identifiers;

/// <summary>
/// Interface for Bank Account Number validators.
/// </summary>
public interface IBankAccountValidator : IValidator<string>
{
    /// <summary>
    /// Gets the country code (ISO 3166-1 alpha-2) for this validator.
    /// </summary>
    string CountryCode { get; }
}
