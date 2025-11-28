using Finova.Core.Models;

namespace Finova.Core.Interfaces
{
    /// <summary>
    /// Interface for parsing IBANs into country-specific details.
    /// </summary>
    public interface IIbanParser
    {
        /// <summary>
        /// ISO country code this service handles (e.g., "BE", "FR", "GB").
        /// Returns null for generic IBAN service.
        /// </summary>
        string? CountryCode { get; }

        /// <summary>
        /// Parses an IBAN and returns country-specific detailed information.
        /// </summary>
        /// <param name="iban">The IBAN to parse</param>
        /// <returns>
        /// IbanDetails with country-specific fields if the IBAN is valid;
        /// otherwise, null.
        /// </returns>
        IbanDetails? ParseIban(string? iban);
    }
}
