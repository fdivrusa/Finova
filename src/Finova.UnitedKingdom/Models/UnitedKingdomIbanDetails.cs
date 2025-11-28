using Finova.Core.Models;

namespace Finova.UnitedKingdom.Models
{
    /// <summary>
    /// UK-specific IBAN details.
    /// UK IBAN format: GB + 2 check digits + 4 bank code (BIC) + 6 sort code + 8 account number.
    /// Example: GB29MIDL40051512345678
    /// </summary>
    public record UnitedKingdomIbanDetails : IbanDetails
    {
        /// <summary>
        /// 4-letter bank identifier code (partial BIC).
        /// Example: "MIDL" (HSBC), "NWBK" (NatWest), "BARC" (Barclays).
        /// Position: 5-8
        /// </summary>
        public required string BankIdentifier { get; init; }

        /// <summary>
        /// 6-digit sort code identifying the bank branch.
        /// Format: XX-XX-XX
        /// Example: "400515" → 40-05-15
        /// Position: 9-14
        /// </summary>
        public required string SortCode { get; init; }

        /// <summary>
        /// 8-digit account number.
        /// Position: 15-22
        /// </summary>
        public required string UkAccountNumber { get; init; }

        /// <summary>
        /// Formatted sort code (XX-XX-XX).
        /// </summary>
        public string FormattedSortCode => $"{SortCode[..2]}-{SortCode.Substring(2, 2)}-{SortCode.Substring(4, 2)}";
    }
}
