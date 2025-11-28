using Finova.Core.Models;

namespace Finova.France.Models
{
    /// <summary>
    /// French-specific IBAN details.
    /// FR IBAN format: FR + 2 check digits + 5 bank code + 5 branch code + 11 account number + 2 RIB key.
    /// Example: FR1420041010050500013M02606
    /// </summary>
    public record FranceIbanDetails : IbanDetails
    {
        /// <summary>
        /// 5-digit bank code (Code Banque).
        /// Position: 5-9
        /// </summary>
        public required string BankCodeFr { get; init; }

        /// <summary>
        /// 5-digit branch code (Code Guichet).
        /// Position: 10-14
        /// </summary>
        public required string BranchCodeFr { get; init; }

        /// <summary>
        /// 11-character account number (Numéro de Compte).
        /// Can contain letters and digits.
        /// Position: 15-25
        /// </summary>
        public required string AccountNumberFr { get; init; }

        /// <summary>
        /// 2-digit RIB check key (Clé RIB).
        /// Position: 26-27
        /// </summary>
        public required string RibKey { get; init; }

        /// <summary>
        /// Full RIB (Relevé d'Identité Bancaire) formatted.
        /// Format: BBBBB GGGGG CCCCCCCCCCC KK
        /// </summary>
        public string FormattedRib => $"{BankCodeFr} {BranchCodeFr} {AccountNumberFr} {RibKey}";
    }
}
