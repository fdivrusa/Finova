using Finova.Core.Common;

namespace Finova.Core.Iban;

public interface IIbanValidator
{
    /// <summary>
    /// ISO country code (ex: "BE", "FR")
    /// </summary>
    string CountryCode { get; }

    /// <summary>
    /// Validates an IBAN for the specific country.
    /// </summary>
    ValidationResult Validate(string? iban);
}
