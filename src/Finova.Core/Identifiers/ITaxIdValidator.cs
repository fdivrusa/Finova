using Finova.Core.Common;

namespace Finova.Core.Identifiers;

/// <summary>
/// Interface for Tax Identification Number validators (e.g., EIN, PAN, TFN).
/// </summary>
public interface ITaxIdValidator : IValidator<string>
{
    /// <summary>
    /// Gets the country code (ISO 3166-1 alpha-2) for this validator.
    /// </summary>
    string CountryCode { get; }
}
