using System.Diagnostics.CodeAnalysis;

namespace Finova.Core.Interfaces
{
    /// <summary>
    /// Interface for IBAN validation, formatting, and parsing operations.
    /// Supports ISO 13616 IBAN structure.
    /// </summary>
    public interface IIbanService
    {
        /// <summary>
        /// ISO country code this service handles (e.g., "BE", "FR", "GB").
        /// Returns null for generic IBAN service.
        /// </summary>
        string? CountryCode { get; }

        /// <summary>
        /// Validates an IBAN (structure and checksum).
        /// </summary>
        /// <param name="iban">The IBAN to validate</param>
        /// <returns>True if valid, false otherwise</returns>
        bool IsValidIban([NotNullWhen(true)] string? iban);

        /// <summary>
        /// Formats an IBAN with spaces every 4 characters for display.
        /// </summary>
        /// <param name="iban">The IBAN to format</param>
        /// <returns>Formatted IBAN (e.g., "BE68 5390 0754 7034")</returns>
        string FormatIban(string? iban);

        /// <summary>
        /// Normalizes an IBAN by removing spaces and converting to uppercase.
        /// </summary>
        /// <param name="iban">The IBAN to normalize</param>
        /// <returns>Normalized IBAN</returns>
        string NormalizeIban(string? iban);

        /// <summary>
        /// Gets the country code from an IBAN.
        /// </summary>
        /// <param name="iban">The IBAN</param>
        /// <returns>Two-letter country code or empty string if invalid</returns>
        string GetCountryCode(string? iban);

        /// <summary>
        /// Gets the check digits from an IBAN.
        /// </summary>
        /// <param name="iban">The IBAN</param>
        /// <returns>Check digits (0-97) or 0 if invalid</returns>
        int GetCheckDigits(string? iban);

        /// <summary>
        /// Validates the IBAN checksum using modulo 97 (ISO 7064).
        /// </summary>
        /// <param name="iban">The IBAN to validate</param>
        /// <returns>True if checksum is valid, false otherwise</returns>
        bool ValidateChecksum([NotNullWhen(true)] string? iban);
    }
}
