using Finova.Core.Common;

namespace Finova.Core.Identifiers;

/// <summary>
/// Interface for Bank Routing Code validators (e.g., ABA Routing Number, BSB, IFSC).
/// </summary>
public interface IBankRoutingValidator : IValidator<string>
{
    /// <summary>
    /// Gets the country code (ISO 3166-1 alpha-2) for this validator.
    /// </summary>
    string CountryCode { get; }
}
