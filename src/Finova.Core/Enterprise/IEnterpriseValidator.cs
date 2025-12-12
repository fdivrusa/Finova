using Finova.Core.Common;

namespace Finova.Core.Enterprise;

/// <summary>
/// Interface for validating Enterprise/Business Registration Numbers (e.g., SIRET, Firmenbuchnummer, KBO).
/// </summary>
public interface IEnterpriseValidator : IValidator<string>
{
    /// <summary>
    /// ISO country code (ex: "BE", "AT", "FR")
    /// </summary>
    string CountryCode { get; }
}
