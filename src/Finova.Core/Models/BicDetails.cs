namespace Finova.Core.Models
{
    /// <summary>
    /// Represents a parsed Business Identifier Code (BIC/SWIFT).
    /// ISO 9362 Standard: AAAA BB CC DDD
    /// </summary>
    public record BicDetails
    {
        /// <summary>
        /// The normalized BIC (always 11 characters, padded with XXX if necessary).
        /// </summary>
        public required string Bic { get; init; }

        /// <summary>
        /// 4-letter bank code (Indices 0-3).
        /// </summary>
        public required string BankCode { get; init; }

        /// <summary>
        /// 2-letter ISO country code (Indices 4-5).
        /// </summary>
        public required string CountryCode { get; init; }

        /// <summary>
        /// 2-character location code (Indices 6-7).
        /// </summary>
        public required string LocationCode { get; init; }

        /// <summary>
        /// 3-character branch code (Indices 8-10).
        /// Returns "XXX" if the input was an 8-character BIC.
        /// </summary>
        public required string BranchCode { get; init; }

        /// <summary>
        /// True if this references the main office (BranchCode is "XXX").
        /// </summary>
        public bool IsMainBranch => BranchCode.Equals("XXX", StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Indicates if this is likely a Test BIC (Location code ends in '0').
        /// </summary>
        public bool IsTestBic => LocationCode.Length == 2 && LocationCode[1] == '0';

        public bool IsValid { get; init; }
    }
}