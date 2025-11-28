using Finova.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finova.Belgium.Models
{
    /// <summary>
    /// Belgian-specific IBAN details.
    /// BE IBAN format: BE + 2 check digits + 12 digits (3-digit bank code + 7-digit account + 2-digit check).
    /// Example: BE68539007547034
    /// </summary>
    public record BelgiumIbanDetails : IbanDetails
    {
        /// <summary>
        /// 3-digit bank code.
        /// Position: 5-7
        /// </summary>
        public required string BankCodeBe { get; init; }

        /// <summary>
        /// 7-digit account number.
        /// Position: 8-14
        /// </summary>
        public required string AccountNumberBe { get; init; }

        /// <summary>
        /// 2-digit Belgian check key (MOD 97).
        /// Position: 15-16
        /// </summary>
        public required string BelgianCheckKey { get; init; }

        /// <summary>
        /// Formatted Belgian account number: XXX-XXXXXXX-YY
        /// </summary>
        public string FormattedBelgianAccount => $"{BankCodeBe}-{AccountNumberBe}-{BelgianCheckKey}";
    }
}
