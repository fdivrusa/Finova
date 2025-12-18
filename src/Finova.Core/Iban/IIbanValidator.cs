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
    /// <param name="iban">The IBAN string to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    /// <example>
    /// <code>
    /// var validator = new BelgiumIbanValidator();
    /// var result = validator.Validate("BE68539007547034");
    /// </code>
    /// </example>
    ValidationResult Validate(string? iban);
}
