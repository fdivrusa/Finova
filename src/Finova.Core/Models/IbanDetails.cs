namespace Finova.Core.Models
{
    /// <summary>
    /// Base class for IBAN details containing common fields across all countries.
    /// </summary>
    public record IbanDetails
    {
        /// <summary>
        /// The original IBAN (normalized).
        /// </summary>
        public required string Iban { get; init; }

        /// <summary>
        /// ISO country code (e.g., "GB", "FR", "BE").
        /// </summary>
        public required string CountryCode { get; init; }

        /// <summary>
        /// Check digits (2 digits after country code).
        /// </summary>
        public required string CheckDigits { get; init; }

        /// <summary>
        /// Bank code (format varies by country).
        /// </summary>
        public string? BankCode { get; init; }

        /// <summary>
        /// Branch/Sort code (if applicable).
        /// </summary>
        public string? BranchCode { get; init; }

        /// <summary>
        /// Account number.
        /// </summary>
        public string? AccountNumber { get; init; }

        /// <summary>
        /// National check key (if applicable, e.g., French RIB key).
        /// </summary>
        public string? NationalCheckKey { get; init; }

        /// <summary>
        /// Indicates if the IBAN structure is valid.
        /// </summary>
        public bool IsValid { get; init; }
    }
}
